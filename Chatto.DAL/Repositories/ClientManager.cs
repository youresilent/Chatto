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
	public class ClientManager : IClientManager
	{
		public ApplicationContext DataBase { get; set; }
		
		public ClientManager(ApplicationContext db)
		{
			DataBase = db;
		}

		public void Create(ClientProfile item)
		{
			DataBase.ClientProfiles.Add(item);
			DataBase.SaveChanges();
		}

		public void Dispose()
		{
			DataBase.Dispose();
		}
	}
}
