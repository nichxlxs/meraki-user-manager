using System.Net.Http.Json;

namespace MerakiUserManager
{
	public static class MerakiApi
	{

		private static readonly HttpClient client = new HttpClient();

		static MerakiApi()
		{
			client.DefaultRequestHeaders.Add("Accept", "application/json");
		}

		private static void AddApiKey(string apiKey)
		{
			if (client.DefaultRequestHeaders.Contains("X-Cisco-Meraki-API-Key"))
			{
				client.DefaultRequestHeaders.Remove("X-Cisco-Meraki-API-Key");
			}
			client.DefaultRequestHeaders.Add("X-Cisco-Meraki-API-Key", apiKey);
		}

		public static async Task<List<Organization>> GetOrganizations(string apiKey)
		{
			AddApiKey(apiKey);
			var url = "https://api.meraki.com/api/v1/organizations";
			var response = await client.GetAsync(url);
			response.EnsureSuccessStatusCode();
			var organizations = await response.Content.ReadFromJsonAsync<List<Organization>>();
			return organizations ?? new List<Organization>();
		}

		public static async Task<List<Admin>> GetAdminsFromOrganization(string apiKey, string orgId)
		{
			AddApiKey(apiKey);
			var url = $"https://api.meraki.com/api/v1/organizations/{orgId}/admins";
			var response = await client.GetAsync(url);
			response.EnsureSuccessStatusCode();
			var admins = await response.Content.ReadFromJsonAsync<List<Admin>>();
			return admins ?? new List<Admin>();
		}

		public static async Task<List<Organization>> GetOrganizationsFromAdmin(string apiKey, string adminEmail, Scope scope)
		{
			AddApiKey(apiKey);
			var organizations = scope.Organizations;

			var organizationsWithAdmin = new List<Organization>();
			foreach (var org in organizations)
			{
				var admins = await GetAdminsFromOrganization(apiKey, org.Id);
				if (admins.Exists(a => a.Email == adminEmail))
				{
					organizationsWithAdmin.Add(org);
				}
			}

			return organizationsWithAdmin;

		}

		public static async Task<List<Admin>> GetAllUsers(string apiKey)
		{
			AddApiKey(apiKey);
			var organizations = await GetOrganizations(apiKey);

			var allAdmins = new List<Admin>();

			foreach (var org in organizations)
			{
				var admins = await GetAdminsFromOrganization(apiKey, org.Id);
				foreach (var admin in admins)
				{
					if (!allAdmins.Exists(a => a.Email == admin.Email))
					{
						allAdmins.Add(admin);
					}
				}
			}
			
			return allAdmins;
		}

		public static async Task AddUser(string apiKey, string orgId, string email, string name)
		{
			AddApiKey(apiKey);
			var url = $"https://api.meraki.com/api/v1/organizations/{orgId}/admins";
			var admin = new { email, name, orgAccess = "full" };
			var response = await client.PostAsJsonAsync(url, admin);

			if (response.IsSuccessStatusCode)
			{
				Console.WriteLine($"User {name} added to organization {orgId}.");
			}

			else
			{
				var error = await response.Content.ReadAsStringAsync();
				Console.WriteLine($"Failed to add user to organization {orgId}: {error}");
			}
		}

		public static async Task RemoveUser(string apiKey, string orgId, string email)
		{
			AddApiKey(apiKey);
			var url = $"https://api.meraki.com/api/v1/organizations/{orgId}/admins";

			var admins = await client.GetFromJsonAsync<List<Admin>>(url);
			var admin = admins?.Find(a => a.Email == email);
			if (admin == null)
			{
				Console.WriteLine($"User {email} not found in organization {orgId}.");
				return;
			}

			url = $"https://api.meraki.com/api/v1/organizations/{orgId}/admins/{admin.Id}";
			var response = await client.DeleteAsync(url);
			
			if (response.IsSuccessStatusCode)
			{
				Console.WriteLine($"User {email} removed from organization {orgId}.");
			}
			else
			{
				var error = await response.Content.ReadAsStringAsync();
				Console.WriteLine($"Failed to remove user from organization {orgId}: {error}");
			}
			

		}
	}
}