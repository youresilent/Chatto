using Chatto.DAL.Entities;
using System;
using System.Collections.Generic;

namespace Chatto.DAL.Interfaces
{
	public interface IMessageManager : IDisposable
	{
		List<ClientMessage> GetMessages(string currentUserName, string friendUserName);

		List<ClientMessage> GetMessagesForRemoval(string userName);
	}
}
