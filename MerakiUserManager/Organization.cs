using System.Text.Json.Serialization;

namespace MerakiUserManager
{
	public class Organization
	{
		[JsonPropertyName("id")]
		public required string Id { get; set; }
		[JsonPropertyName("name")]
		public required string Name { get; set; }
	}
}
