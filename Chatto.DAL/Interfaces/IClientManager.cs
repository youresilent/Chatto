using Chatto.DAL.Entities;
using System;
using System.Collections.Generic;

namespace Chatto.DAL.Interfaces
{
	public interface IClientManager : IDisposable
	{
		void Create(ClientProfile item);

		void Remove(ClientProfile item);

		void RemovePendingFriend(ClientPendingFriend item);

		List<string> GetOutgoingPendingFriends(string id);
	}
}
