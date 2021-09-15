using Chatto.BLL.DTO;
using Chatto.BLL.Infrastructure;
using Chatto.BLL.Interfaces;
using Chatto.DAL.Entities;
using Chatto.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatto.BLL.Services
{
	public class MessageService : IMessageService
	{
		IUnitOfWork DataBase { get; set; }

		public MessageService(IUnitOfWork uow)
		{
			DataBase = uow;
		}

		public async Task<OperationDetails> Create(MessageDTO messageDTO)
		{
			try
			{
				DataBase.MessageManager.Create(GetClientMessage(messageDTO));
				await DataBase.SaveAsync();
			}
			catch
			{
				return new OperationDetails(false, StatusStringsResource.CreateMessage_Failure, "messageDTO");
			}

			return new OperationDetails(true, StatusStringsResource.CreateMessage_OK, "");
		}

		public async Task<OperationDetails> Remove(MessageDTO messageDTO)
		{
			try
			{
				DataBase.MessageManager.Remove(GetClientMessage(messageDTO));
				await DataBase.SaveAsync();
			}
			catch
			{
				return new OperationDetails(false, StatusStringsResource.RemoveMessage_Failure, "messageDTO");
			}

			return new OperationDetails(true, StatusStringsResource.RemoveMessage_OK, "");
		}

		public List<MessageDTO> GetMessages(string currentUserName, string friendUserName)
		{
			var clientMessagesList = DataBase.MessageManager.GetMessages(currentUserName, friendUserName);
			var messageDTOList = new List<MessageDTO>();

			foreach (var message in clientMessagesList)
			{
				messageDTOList.Add(GetMessageDTO(message));
			}

			return messageDTOList;
		}

		public void RemoveMessages(string userName)
		{
			foreach (var message in DataBase.MessageManager.GetMessagesForRemoval(userName))
			{
				DataBase.MessageManager.Remove(message);
			}
		}

		public void Dispose()
		{
			DataBase.Dispose();
		}

		private ClientMessage GetClientMessage(MessageDTO messageDTO)
		{
			var clientMessage = new ClientMessage
			{
				Id = messageDTO.Id,
				Sender = messageDTO.Sender,
				Recipient = messageDTO.Recipient,
				Message = messageDTO.Message,
				SendDateTime = messageDTO.SendDateTime
			};

			return clientMessage;
		}

		private MessageDTO GetMessageDTO(ClientMessage clientMessage)
		{
			var messageDTO = new MessageDTO
			{
				Id = clientMessage.Id,
				Sender = clientMessage.Sender,
				Recipient = clientMessage.Recipient,
				Message = clientMessage.Message,
				SendDateTime = clientMessage.SendDateTime
			};

			return messageDTO;
		}

	}
}
