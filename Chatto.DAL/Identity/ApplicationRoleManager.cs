using Chatto.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Chatto.DAL.Identity
{
	public class ApplicationRoleManager : RoleManager<ApplicationRole>
	{
		public ApplicationRoleManager(RoleStore<ApplicationRole> store) : base(store)
		{ }
	}
}
