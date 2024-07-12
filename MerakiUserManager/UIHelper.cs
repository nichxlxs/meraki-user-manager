namespace MerakiUserManager
{
	public static class UIHelper
	{
		public static void DisplayWelcomeMessage()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Welcome to the Meraki User Manager!");
			Console.ResetColor();
		}

		public static string PromptForApiKey()
		{
			Console.Write("Enter your Meraki API key: ");
			return Console.ReadLine();
		}

		public static void DisplayInfoModeHelp()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("\nInfo Mode Commands:");
			Console.WriteLine("list orgs - List all organizations");
			Console.WriteLine("list admins <orgid> - List all admins for a specific organization");
			Console.WriteLine("list admins all - List all admins for all organizations");
			Console.WriteLine("list orgs <admin email> - List all organizations a specific admin is in");
			Console.WriteLine("mode modify - Enter modify mode to bulk add/remove users");
			Console.WriteLine("exit - Exit the application");
			Console.ResetColor();
		}

		public static void DisplayModifyModeHelp()
		{
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.WriteLine("\nModify Mode Commands:");
			Console.WriteLine("scope list - List the current scope and all affected organizations (by id and name)");
			Console.WriteLine("scope add <all/orgid>");
			Console.WriteLine("scope remove <all/orgid>");
			Console.WriteLine("create admin <email> <name>");
			Console.WriteLine("delete admin <email>");
			Console.WriteLine("mode info - Enter info mode");
			Console.WriteLine("exit - Exit the application");
			Console.ResetColor();
		}

		public static void DisplayOrganizations(List<Organization> organizations)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			foreach (var org in organizations)
			{
				Console.WriteLine($"ID: {org.Id}, Name: {org.Name}");
			}
			Console.ResetColor();
		}

		public static void DisplayAdmins(List<Admin> admins)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			foreach (var admin in admins)
			{
				Console.WriteLine($"ID: {admin.Id}, Email: {admin.Email}");
			}
			Console.ResetColor();
		}

		public static void DisplayScope(List<Organization> organizations, Scope scope)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Scope:");
			foreach (var orgId in scope.OrgScope)
			{
				var org = organizations.Find(o => o.Id == orgId);
				if (org != null)
				{
					Console.WriteLine($"ID: {org.Id}, Name: {org.Name}");
				}
			}
			Console.ResetColor();
		}

		public static void DisplaySuccessMessage(string message)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(message);
			Console.ResetColor();
		}

		public static void DisplayErrorMessage(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
		}

		public static void DisplayExitMessage()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Thank you for using the Meraki User Manager. Goodbye!");
			Console.ResetColor();
		}
	}
}
