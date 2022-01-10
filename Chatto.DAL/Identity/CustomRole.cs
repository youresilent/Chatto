using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Chatto.DAL.Identity
{
    public class CustomRole : IdentityRole<Guid, CustomUserRole>
    {
        public CustomRole() { }

        public CustomRole(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
