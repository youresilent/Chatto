using Autofac;
using Autofac.Integration.Mvc;
using Chatto.BLL.Interfaces;
using Chatto.BLL.Services;
using Chatto.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chatto.BLL.Services
{
	public class AutofacBLLConfig
	{
		public static ContainerBuilder ConfigureBuilderInBLL()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<MessageService>().As<IMessageService>().WithParameter("uow", new IdentityUnitOfWork());

			return builder;
		}
	}
}