using Chatto.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace Chatto.DAL.Identity
{
	public class ApplicationRoleManager : RoleManager<ApplicationRole>
	{
		public ApplicationRoleManager(IRoleStore<ApplicationRole> store) : base(store)
		{ }
	}
}
