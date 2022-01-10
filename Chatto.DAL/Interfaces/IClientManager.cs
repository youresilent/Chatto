using Chatto.DAL.Entities;
using System;
using System.Collections.Generic;

namespace Chatto.DAL.Interfaces
{
	public interface IClientManager : IDisposable
	{
		void RemovePendingFriend(ClientPendingFriend item);

		List<Guid> GetOutgoingPendingFriends(Guid id);
	}
}
