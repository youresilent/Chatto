using System.Collections.Generic;

namespace Chatto.DAL.Entities
{
	public class ClientProfile
	{
		public string Id { get; set; }

		public string RealName { get; set; }
		public string Adress { get; set; }
		public string Gender { get; set; }
		public int Age { get; set; }

		public string Friends { get; set; }

		public virtual ApplicationUser ApplicationUser { get; set; }

		public virtual ICollection<ClientPendingFriend> ClientPendingFriends { get; set; }

		public ClientProfile()
		{
			ClientPendingFriends = new HashSet<ClientPendingFriend>();
		}
	}
}