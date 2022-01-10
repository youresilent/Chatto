using Chatto.DAL.Identity;
using System;
using System.Threading.Tasks;

namespace Chatto.DAL.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		ApplicationUserManager UserManager { get; }

        ApplicationRoleManager RoleManager { get; }

        IClientManager ClientManager { get; }

		IMessageManager MessageManager { get; }

		IDBManager DBManager { get; }

		Task SaveAsync();
	}
}
