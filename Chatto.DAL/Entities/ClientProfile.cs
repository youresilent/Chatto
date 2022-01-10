using System;
using System.Collections.Generic;

namespace Chatto.DAL.Entities
{
	public class ClientProfile
	{
		public Guid Id { get; set; }

		public string RealName { get; set; }
		public string Adress { get; set; }
		public string Gender { get; set; }
		public int Age { get; set; }


		public virtual ApplicationUser ApplicationUser { get; set; }

		public virtual ICollection<ClientPendingFriend> ClientSenderPendingFriends { get; set; }
		public virtual ICollection<ClientPendingFriend> ClientReceiverPendingFriends { get; set; }

		public virtual ICollection<ClientFriend> ClientFriends { get; set; }
	}
}