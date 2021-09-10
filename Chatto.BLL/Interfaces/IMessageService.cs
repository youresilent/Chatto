using Chatto.BLL.DTO;
using Chatto.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatto.BLL.Interfaces
{
	public interface IMessageService : IDisposable
	{
		Task<OperationDetails> Create(MessageDTO messageDTO);

		Task<OperationDetails> Remove(MessageDTO messageDTO);

		List<MessageDTO> GetMessages(string currentUserName, string friendUserName);

		void RemoveMessages(string userName);
	}
}