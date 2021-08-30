using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatto.BLL.DTO
{
	public class MessageDTO
	{
		public string Id { get; set; }

		public string Sender { get; set; }

		public string Recipient { get; set; }

		public string Message { get; set; }

		public DateTime SendDateTime { get; set; }
	}
}
