using Chatto.BLL.DTO;
using Chatto.BLL.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Chatto.BLL.Interfaces
{
	public interface IMessageService : IDisposable
	{
		Task<OperationDetails> Create(MessageDTO messageDTO);

		Task<OperationDetails> Remove(MessageDTO messageDTO);
	}
}