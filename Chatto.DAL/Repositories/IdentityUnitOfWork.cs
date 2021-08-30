using Chatto.DAL.EF;
using Chatto.DAL.Entities;
using Chatto.DAL.Identity;
using Chatto.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;

namespace Chatto.DAL.Repositories
{
	public class IdentityUnitOfWork : IUnitOfWork
	{
		private bool disposed = false;

		private readonly ApplicationContext DataBase;

		private readonly ApplicationUserManager userManager;
		private readonly ApplicationRoleManager roleManager;
		private readonly IClientManager clientManager;
		private readonly IMessageManager messageManager;

		public IdentityUnitOfWork()
		{
			DataBase = new ApplicationContext();

			userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(DataBase));
			roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(DataBase));

			clientManager = new ClientManager(DataBase);
			messageManager = new MessageManager(DataBase);
		}

		public ApplicationUserManager UserManager { get { return userManager; } }

		public ApplicationRoleManager RoleManager { get { return roleManager; } }

		public IClientManager ClientManager { get { return clientManager; } }

		public IMessageManager MessageManager { get { return messageManager; } }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					userManager.Dispose();
					roleManager.Dispose();

					clientManager.Dispose();
					messageManager.Dispose();
				}

				disposed = true;
			}
		}

		public async Task SaveAsync()
		{
			await DataBase.SaveChangesAsync();
		}
	}
}
