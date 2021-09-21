﻿using System.Collections.Generic;

namespace Chatto.DAL.Entities
{
	public class ClientPendingFriend
	{
		public int Id { get; set; }

		public string Id_Receiver { get; set; }

		public string Id_Sender { get; set; }

		public virtual ClientProfile ClientProfile { get; set; }
	}
}
