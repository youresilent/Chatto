﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatto.DAL.Entities
{
	public class ClientFriend
	{
		public int Id { get; set; }

		public string Friend_Id1 { get; set; }
		public virtual ClientProfile Friend_Profile1 { get; set; }

		public string Friend_Id2 { get; set; }
	}
}
