using Chatto.BLL.Interfaces;
using Chatto.BLL.Services;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

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
				LoginPath = new PathString("/Account/Login")
			});
		}

		private IUserService CreateUserService()
		{
			return serviceCreator.CreateUserService("IdentityDb");
		}
	}
}
