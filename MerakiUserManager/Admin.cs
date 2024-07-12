using System.Text.Json.Serialization;

namespace MerakiUserManager
{
	public class Admin
	{
		[JsonPropertyName("id")]
		public required string Id { get; set; }
		
		[JsonPropertyName("email")]
		public required string Email { get; set; }
		
		[JsonPropertyName("name")]
		public required string Name { get; set; }
	}
}
