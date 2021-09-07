using Chatto.DAL.EF;
using Chatto.DAL.Entities;
using Chatto.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatto.DAL.Repositories
{
	public class MessageManager : IMessageManager
	{
		public ApplicationContext DataBase { get; set; }

		public MessageManager(ApplicationContext db)
		{
			DataBase = db;
		}

		public void Create(ClientMessage message)
		{
			DataBase.ClientMessages.Add(message);
			DataBase.SaveChanges();
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

			//исправить сообщение в БД

			var userMessagesList = userMessagesEnumerable.ToList();

			return userMessagesList;
		}

		public void Remove(ClientMessage message)
		{
			DataBase.ClientMessages.Remove(message);
			DataBase.SaveChanges();
		}
		public void Dispose()
		{
			DataBase.Dispose();
		}
	}
}
