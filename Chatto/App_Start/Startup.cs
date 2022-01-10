using Chatto.BLL.Interfaces;
using Chatto.BLL.Services;
using Chatto.DAL.Entities;
using Chatto.DAL.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

[assembly: OwinStartup(typeof(Chatto.App_Start.Startup))]

namespace Chatto.App_Start
{
	public class Startup
	{
		readonly IServiceCreator serviceCreator = new ServiceCreator();
		public void Configuration(IAppBuilder app)
		{
			app.CreatePerOwinContext<IUserService>(CreateUserService);
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Account/Login"),
				Provider = new CookieAuthenticationProvider
				{
					OnValidateIdentity = SecurityStampValidator
						.OnValidateIdentity<ApplicationUserManager, ApplicationUser, Guid>(
					validateInterval: TimeSpan.FromMinutes(30),
					regenerateIdentityCallback: (manager, user) =>
						user.GenerateUserIdentityAsync(manager),
					getUserIdCallback: (id) => Guid.Parse(id.GetUserId()))
				}
			});

			app.MapSignalR();
		}

		private IUserService CreateUserService()
		{
			return serviceCreator.CreateUserService();
		}
	}
}
