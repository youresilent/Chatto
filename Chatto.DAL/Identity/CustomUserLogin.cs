using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Chatto.DAL.Identity
{
    public class CustomUserLogin : IdentityUserLogin<Guid>
    {
    }
}
