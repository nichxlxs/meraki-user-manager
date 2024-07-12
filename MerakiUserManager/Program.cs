namespace MerakiUserManager
{
	class Program
	{

		static async Task Main(string[] args)
		{
			UIHelper.DisplayWelcomeMessage();
			
			string apiKey = UIHelper.PromptForApiKey();
			Scope scope = new Scope();
			scope.Organizations = await MerakiApi.GetOrganizations(apiKey);

			while (true)
			{
				await InfoMode(apiKey, scope);
			}
		}

		private static async Task InfoMode(string apiKey, Scope scope)
		{
			while (true)
			{
				UIHelper.DisplayInfoModeHelp();
				Console.Write("Command: ");
				string command = Console.ReadLine();
				Console.WriteLine();
				var commandArgs = command.Split(' ');

				switch (commandArgs[0].ToLower())
				{
					case "list":
						await HandleListCommands(apiKey, commandArgs, scope);
						break;

					case "mode":
						if (commandArgs.Length > 1 && commandArgs[1].ToLower() == "modify")
						{
							await ModifyMode(apiKey, scope);
						}
						else
						{
							UIHelper.DisplayErrorMessage("Invalid mode command. Use mode <modify/info>.");
						}
						break;

					case "exit":
						UIHelper.DisplayExitMessage();
						Environment.Exit(0);
						break;

					default:
						UIHelper.DisplayErrorMessage("Invalid command.");
						break;
				}
			}
		}

		private static async Task HandleListCommands(string apiKey, string[] commandArgs, Scope scope)
		{
			if (commandArgs.Length < 2)
			{
				UIHelper.DisplayErrorMessage("Invalid list command.");
				return;
			}

			switch (commandArgs[1].ToLower())
			{
				case "orgs":
					if (commandArgs.Length > 2)
					{
						var adminEmail = commandArgs[2];
						var orgsForAdmin = await MerakiApi.GetOrganizationsFromAdmin(apiKey, adminEmail, scope);
						UIHelper.DisplayOrganizations(orgsForAdmin);
					}
					else
					{
						var organizations = scope.Organizations;
						UIHelper.DisplayOrganizations(organizations);
					}
					break;

				case "admins":
					if (commandArgs.Length > 2)
					{
						if (commandArgs[2].ToLower() == "all")
						{
							var allAdmins = await MerakiApi.GetAllUsers(apiKey);
							UIHelper.DisplayAdmins(allAdmins);
						}
						else
						{
							var orgId = commandArgs[2];
							var admins = await MerakiApi.GetAdminsFromOrganization(apiKey, orgId);
							UIHelper.DisplayAdmins(admins);
						}
					}
					else
					{
						UIHelper.DisplayErrorMessage("Invalid list admins command.");
					}
					break;

				default:
					UIHelper.DisplayErrorMessage("Invalid list command.");
					break;
			}
		}

		private static async Task ModifyMode(string apiKey, Scope scope)
		{

			while (true)
			{
				UIHelper.DisplayModifyModeHelp();
				Console.Write("Command: ");
				string command = Console.ReadLine();
				Console.WriteLine();
				var commandArgs = command.Split(' ');

				switch (commandArgs[0].ToLower())
				{
					case "scope":
						await HandleScopeCommands(apiKey, commandArgs, scope);
						break;

					case "create":
						if (commandArgs.Length > 2 && commandArgs[1].ToLower() == "admin")
						{
							if (commandArgs.Length > 3)
							{
								string email = commandArgs[2];
								string name = string.Join(' ', commandArgs.Skip(3));
								await BulkAddUser(apiKey, scope, email, name);
							}
							else
							{
								UIHelper.DisplayErrorMessage("Invalid create admin command.");
							}
						}
						break;

					case "delete":
						if (commandArgs.Length > 2 && commandArgs[1].ToLower() == "admin")
						{
							if (commandArgs.Length > 2)
							{
								string email = commandArgs[2];
								await BulkRemoveUser(apiKey, scope, email);
							}
							else
							{
								UIHelper.DisplayErrorMessage("Invalid delete admin command.");
							}
						}
						break;

					case "mode":
						if (commandArgs.Length > 1 && commandArgs[1].ToLower() == "info")
						{
							return;
						}
						else
						{
							UIHelper.DisplayErrorMessage("Invalid mode command.");
						}
						break;

					case "exit":
						UIHelper.DisplayExitMessage();
						Environment.Exit(0);
						break;

					default:
						UIHelper.DisplayErrorMessage("Invalid command.");
						break;
				}
			}
		}

		private static async Task HandleScopeCommands(string apiKey, string[] commandArgs, Scope scope)
		{

			var organizations = scope.Organizations;

			if (commandArgs.Length < 2)
			{
				UIHelper.DisplayErrorMessage("Invalid scope command.");
				return;
			}

			switch (commandArgs[1].ToLower())
			{
				case "list":
					UIHelper.DisplayScope(organizations, scope);
					break;

				case "add":
					if (commandArgs.Length > 2)
					{
						if (commandArgs[2].ToLower() == "all")
						{
							foreach (var org in organizations)
							{

								scope.Add(org.Id);
							}
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine($"Added all orgs to the scope");
							Console.ResetColor();
						}
						else
						{
							var orgid = commandArgs[2];
							if (!organizations.Exists(o => o.Id == orgid))
							{
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine($"Could not find org {orgid}.");
								Console.ResetColor();
								break;
							}
							
							scope.Add(orgid);
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine($"Added {orgid} to the scope if it exists.");
							Console.ResetColor();
						}
					}
					else
					{
						UIHelper.DisplayErrorMessage("Invalid scope add command.");
					}
					break;

				case "remove":
					if (commandArgs.Length > 2)
					{
						if (commandArgs[2].ToLower() == "all")
						{
							scope.Clear();
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine($"Cleared the scope");
							Console.ResetColor();
						}
						else
						{
							var orgid = commandArgs[2];
							if (!organizations.Exists(o => o.Id == orgid))
							{
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine($"Could not find org {orgid}.");
								Console.ResetColor();
								break;
							}
							
							scope.Remove(orgid);
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine($"Added {orgid} to the scope if it exists.");
							Console.ResetColor();
						}
					}
					else
					{
						UIHelper.DisplayErrorMessage("Invalid scope remove command.");
					}
					break;

				default:
					UIHelper.DisplayErrorMessage("Invalid scope command.");
					break;
			}
		}

		private static async Task BulkAddUser(string apiKey, Scope scope, string email, string name)
		{
			var organizations = scope.Organizations;
			foreach (var org in organizations)
			{
				if (scope.OrgScope.Contains(org.Id))
				{
					try
					{
						await MerakiApi.AddUser(apiKey, org.Id, email, name);
						UIHelper.DisplaySuccessMessage($"User {name} added to organization {org.Name}.");
					}
					catch (Exception ex)
					{
						UIHelper.DisplayErrorMessage($"Failed to add user to organization {org.Name}: {ex.Message}");
					}
				}
			}
		}

		private static async Task BulkRemoveUser(string apiKey, Scope scope, string email)
		{
			var organizations = scope.Organizations;
			foreach (var org in organizations)
			{
				if (scope.OrgScope.Contains(org.Id))
				{
					try
					{
						await MerakiApi.RemoveUser(apiKey, org.Id, email);
						UIHelper.DisplaySuccessMessage($"User {email} removed from organization {org.Name}.");
					}
					catch (Exception ex)
					{
						UIHelper.DisplayErrorMessage($"Failed to remove user from organization {org.Name}: {ex.Message}");
					}
				}
			}
		}
	}
}
