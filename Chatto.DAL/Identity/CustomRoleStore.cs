using Chatto.DAL.EF;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Chatto.DAL.Identity
{
    internal class CustomRoleStore : RoleStore<CustomRole, Guid, CustomUserRole>
    {
        public CustomRoleStore(ApplicationContext context) : base(context)
        {
        }
    }
}
