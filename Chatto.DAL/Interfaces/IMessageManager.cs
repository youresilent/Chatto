using Chatto.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatto.DAL.Interfaces
{
	public interface IMessageManager : IDisposable
	{
		void Create(ClientMessage message);

		void Remove(ClientMessage message);

		List<ClientMessage> GetMessages(string currentUserName, string friendUserName);
	}
}
