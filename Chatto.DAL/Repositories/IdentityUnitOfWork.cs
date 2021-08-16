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

		public IdentityUnitOfWork(string connectionString)
		{
			DataBase = new ApplicationContext(connectionString);

			userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(DataBase));
			roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(DataBase));

			clientManager = new ClientManager(DataBase);
		}

		public ApplicationUserManager UserManager { get { return userManager; } }

		public ApplicationRoleManager RoleManager { get { return roleManager; } }

		public IClientManager ClientManager { get { return clientManager; } }

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
