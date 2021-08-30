using Chatto.BLL.DTO;
using Chatto.BLL.Infrastructure;
using Chatto.BLL.Interfaces;
using Chatto.DAL.Entities;
using Chatto.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatto.BLL.Services
{
	public class MessageService : IMessageService
	{
		private int id = 0;
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
				return new OperationDetails(false, "Message was not saved! SOS", "messageDTO");
			}

			return new OperationDetails(true, "Message was saved successfully!", "");
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
				return new OperationDetails(false, "Message was not removed! :/", "messageDTO");
			}

			return new OperationDetails(true, "Message was removed!", "");
		}

		public void Dispose()
		{
			DataBase.Dispose();
		}

		private ClientMessage GetClientMessage(MessageDTO messageDTO)
		{
			ClientMessage clientMessage = new ClientMessage
			{
				Id = Convert.ToString(++id),
				Sender = messageDTO.Sender,
				Recipient = messageDTO.Recipient,
				Message = messageDTO.Message,
				SendDateTime = messageDTO.SendDateTime
			};

			return clientMessage;
		}

	}
}
