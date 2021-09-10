using Autofac;
using Autofac.Integration.Mvc;
using Chatto.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chatto.App_Start
{
	public class AutofacWEBConfig
	{
		public static void ConfigureContainerInWEB()
		{
			var builder = AutofacBLLConfig.ConfigureBuilderInBLL();

			builder.RegisterControllers(typeof(MvcApplication).Assembly);

			var container = builder.Build();

			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}
	}
}