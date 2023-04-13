using System.Collections.Generic;
using System.Linq;

namespace TRMFrontEnd.Library.Models
{
	public class UserModel
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();

		public string RolesList => string.Join(", ", Roles.Select(x => x.Value));

	}
}