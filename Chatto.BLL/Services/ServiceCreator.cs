using Chatto.BLL.Interfaces;
using Chatto.DAL.Repositories;

namespace Chatto.BLL.Services
{
	public class ServiceCreator : IServiceCreator
	{
		public IUserService CreateUserService()
		{
			return new UserService(new IdentityUnitOfWork());
		}
	}
}
