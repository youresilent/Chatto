using Chatto.DAL.EF;
using Chatto.DAL.Entities;
using Chatto.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chatto.DAL.Repositories
{
	public class ClientManager : IClientManager
	{
		public ApplicationContext DataBase { get; set; }

		public ClientManager(ApplicationContext db)
		{
			DataBase = db;
		}

		public void RemovePendingFriend(ClientPendingFriend item)
		{
			DataBase.ClientPendingFriends.Remove(item);
			DataBase.SaveChanges();
		}

		public List<Guid> GetOutgoingPendingFriends(Guid id)
		{
			var outgoingFriendsEnumerable = DataBase.ClientPendingFriends.ToList()
				.Where(w => w.Id_Sender == id)
				.Select(s => s.Id_Receiver);

			return outgoingFriendsEnumerable.ToList();
		}

		public void Dispose()
		{
			DataBase.Dispose();
		}
	}
}
