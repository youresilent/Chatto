using System;

namespace Chatto.DAL.Entities
{
	public class ClientFriend
	{
		public int Id { get; set; }

		public Guid Friend_Id1 { get; set; }
		public virtual ClientProfile Friend_Profile1 { get; set; }

		public Guid Friend_Id2 { get; set; }
	}
}
