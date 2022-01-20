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
			try
            {
				DataBase.ClientPendingFriends.Remove(item);
				DataBase.SaveChanges();
            }
			catch
            {
				throw;
            }
		}

		public List<Guid> GetOutgoingPendingFriends(Guid id)
		{
			IEnumerable<Guid> outgoingFriendsEnumerable = null;

			try
            {
				outgoingFriendsEnumerable = DataBase.ClientPendingFriends
					.Where(w => w.Id_Sender == id)
					.Select(s => s.Id_Receiver);
            }
			catch
            {
				throw;
            }

			return outgoingFriendsEnumerable.ToList();
		}

		public void Dispose()
		{
			DataBase.Dispose();
		}
	}
}
