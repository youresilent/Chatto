using Chatto.DAL.EF;
using Chatto.DAL.Entities;
using Chatto.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Chatto.DAL.Repositories
{
	public class MessageManager : IMessageManager
	{
		public ApplicationContext DataBase { get; set; }

		public MessageManager(ApplicationContext db)
		{
			DataBase = db;
		}

		public List<ClientMessage> GetMessages(string currentUserName, string friendUserName)
		{
			var userMessagesEnumerable = DataBase.ClientMessages.ToList()
				.Where(m => (m.Sender == currentUserName && m.Recipient == friendUserName) ||
					(m.Sender == friendUserName && m.Recipient == currentUserName))
				.OrderBy(m => m.SendDateTime)
				.Select(m => new ClientMessage
				{
					Id = m.Id,
					Message = m.Message,
					Sender = m.Sender,
					Recipient = m.Recipient,
					SendDateTime = m.SendDateTime
				});

			var userMessagesList = userMessagesEnumerable.ToList();

			return userMessagesList;
		}

		public List<ClientMessage> GetMessagesForRemoval(string userName)
		{
			var userMessagesEnumerable = DataBase.ClientMessages.ToList()
				.Where(m => m.Sender == userName || m.Recipient == userName)
				.OrderBy(m => m.SendDateTime)
				.Select(m => new ClientMessage
				{
					Id = m.Id,
					Message = m.Message,
					Sender = m.Sender,
					Recipient = m.Recipient,
					SendDateTime = m.SendDateTime
				});

			var userMessagesList = userMessagesEnumerable.ToList();

			return userMessagesList;
		}

		public void Dispose()
		{
			DataBase.Dispose();
		}
	}
}
