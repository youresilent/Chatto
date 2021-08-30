using Chatto.DAL.EF;
using Chatto.DAL.Entities;
using Chatto.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatto.DAL.Repositories
{
	public class MessageManager : IMessageManager
	{
		public ApplicationContext DataBase { get; set; }

		public MessageManager(ApplicationContext db)
		{
			DataBase = db;
		}

		public void Create(ClientMessage message)
		{
			DataBase.ClientMessages.Add(message);
			DataBase.SaveChanges();
		}

		public void Dispose()
		{
			DataBase.Dispose();
		}

		public void Remove(ClientMessage message)
		{
			DataBase.ClientMessages.Remove(message);
			DataBase.SaveChanges();
		}
	}
}
