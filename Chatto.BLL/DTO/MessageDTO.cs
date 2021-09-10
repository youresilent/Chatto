using System;

namespace Chatto.BLL.DTO
{
	public class MessageDTO
	{
		public int Id { get; set; }

		public string Sender { get; set; }

		public string Recipient { get; set; }

		public string Message { get; set; }

		public DateTime SendDateTime { get; set; }
	}
}
