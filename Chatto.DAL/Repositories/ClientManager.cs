using Chatto.DAL.EF;
using Chatto.DAL.Entities;
using Chatto.DAL.Interfaces;

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
