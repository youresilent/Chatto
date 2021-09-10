using Chatto.DAL.Entities;
using System;
using System.Collections.Generic;

namespace Chatto.DAL.Interfaces
{
	public interface IMessageManager : IDisposable
	{
		void Create(ClientMessage message);

		void Remove(ClientMessage message);

		List<ClientMessage> GetMessages(string currentUserName, string friendUserName);

		List<ClientMessage> GetMessagesForRemoval(string userName);
	}
}
