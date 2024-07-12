namespace MerakiUserManager
{
	public class Scope
	{
		public List<string> OrgScope { get; } = new List<string>();

		public List<Organization> Organizations = new List<Organization>();

		public void Add(string orgId)
		{
			OrgScope.Add(orgId);
		}

		public void Remove(string orgId)
		{
			OrgScope.Remove(orgId);
		}

		public void Clear()
		{
			OrgScope.Clear();
		}

	}
}
