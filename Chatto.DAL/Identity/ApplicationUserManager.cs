using Chatto.DAL.EF;
using Chatto.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;

namespace Chatto.DAL.Identity
{
	public class ApplicationUserManager : UserManager<ApplicationUser, Guid>
	{
		public ApplicationUserManager(IUserStore<ApplicationUser, Guid> store) : base(store)
		{ }

        public static ApplicationUserManager Create(
        IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                new CustomUserStore(context.Get<ApplicationContext>()));

            manager.UserValidator = new UserValidator<ApplicationUser, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            }; 
            
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser, Guid>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
