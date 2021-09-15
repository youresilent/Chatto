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

		public string PendingFriendsSent { get; set; }

		public string PendingFriendsReceived { get; set; }

		public virtual ApplicationUser ApplicationUser { get; set; }
	}
}