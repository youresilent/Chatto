using Chatto.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace Chatto.DAL.Identity
{
	public class ApplicationUserManager : UserManager<ApplicationUser>
	{
		public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
		{ }
	}
}
