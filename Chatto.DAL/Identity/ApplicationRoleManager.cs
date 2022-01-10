using Chatto.DAL.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;

namespace Chatto.DAL.Identity
{
    public class ApplicationRoleManager : RoleManager<CustomRole, Guid>
    {
        public ApplicationRoleManager(IRoleStore<CustomRole, Guid> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new CustomRoleStore(context.Get<ApplicationContext>()));
        }
    }
}
