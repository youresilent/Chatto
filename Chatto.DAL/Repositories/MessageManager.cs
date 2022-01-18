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
            List<ClientMessage> userMessagesList = null;

            try
            {
                var userMessagesEnumerable = DataBase.ClientMessages
                .Where(m => (m.Sender == currentUserName && m.Recipient == friendUserName) ||
                    (m.Sender == friendUserName && m.Recipient == currentUserName))
                .ToList()
                .OrderBy(m => m.SendDateTime)
                .Select(m => new ClientMessage
                {
                    Id = m.Id,
                    Message = m.Message,
                    Sender = m.Sender,
                    Recipient = m.Recipient,
                    SendDateTime = m.SendDateTime
                })
                .ToList();

                userMessagesList = userMessagesEnumerable.ToList();
            }
            catch
            {
                throw;
            }

            return userMessagesList;
        }

        public List<ClientMessage> GetMessagesForRemoval(string userName)
        {
            List<ClientMessage> userMessagesList = null;

            try
            {
                var userMessagesEnumerable = DataBase.ClientMessages
                .Where(m => m.Sender == userName || m.Recipient == userName)
                .OrderBy(m => m.SendDateTime)
                .ToList()
                .Select(m => new ClientMessage
                {
                    Id = m.Id,
                    Message = m.Message,
                    Sender = m.Sender,
                    Recipient = m.Recipient,
                    SendDateTime = m.SendDateTime
                });

                userMessagesList = userMessagesEnumerable.ToList();
            }
            catch
            {
                throw;
            }

            return userMessagesList;
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
