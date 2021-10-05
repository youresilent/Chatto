using Chatto.DAL.EF;
using Chatto.DAL.Entities;
using Chatto.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatto.DAL.Repositories
{
	public class DBManager : IDBManager
	{
		public ApplicationContext DataBase { get; set; }

		public DBManager(ApplicationContext db)
		{
			DataBase = db;
		}

		public void Create<T>(T item)
		{
			Type type = typeof(T);

			if (type == typeof(ClientProfile))
			{
				DataBase.ClientProfiles.Add(item as ClientProfile);
				DataBase.SaveChanges();
			}

			else if (type == typeof(ClientMessage))
			{
				DataBase.ClientMessages.Add(item as ClientMessage);
				DataBase.SaveChanges();
			}

		}

		public void Dispose()
		{
			DataBase.Dispose();
		}

		public void Remove<T>(T item)
		{
			Type type = typeof(T);

			if (type == typeof(ClientProfile))
			{
				DataBase.ClientProfiles.Remove(item as ClientProfile);
				DataBase.SaveChanges();
			}

			else if (type == typeof(ClientMessage))
			{
				var message = item as ClientMessage;
				var dbEntry = DataBase.ClientMessages.First(m => m.Id == message.Id);

				DataBase.Entry(dbEntry).State = EntityState.Deleted;
				DataBase.SaveChanges();
			}

			else if (type == typeof(ClientFriend))
			{
				var record = item as ClientFriend;
				var dbEntry = DataBase.ClientFriends.First(f => f.Friend_Id1 == record.Friend_Id1 && f.Friend_Id2 == record.Friend_Id2);

				DataBase.Entry(dbEntry).State = EntityState.Deleted;
				DataBase.SaveChanges();
			}
		}
	}
}
