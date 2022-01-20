using Chatto.DAL.Entities;
using System;

namespace Chatto.DAL.Interfaces
{
	public interface IDBManager : IDisposable
	{
		void Create<T>(T item) where T : class;

		void Remove<T>(T item, int optionalEntryId = -1) where T : class;

		ClientFriend FindClientFriend(Guid friendId1, Guid friendId2);
	}
}
