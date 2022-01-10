using Chatto.DAL.EF;
using Chatto.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Chatto.DAL.Identity
{
    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationContext context) : base(context)
        {
        }
    }
}
