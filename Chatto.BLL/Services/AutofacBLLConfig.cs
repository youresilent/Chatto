using Autofac;
using Chatto.BLL.Interfaces;
using Chatto.DAL.EF;
using Chatto.DAL.Interfaces;
using Chatto.DAL.Repositories;

namespace Chatto.BLL.Services
{
	public class AutofacBLLConfig
	{
		public static ContainerBuilder ConfigureBuilderInBLL()
		{
			var builder = new ContainerBuilder();

			builder.RegisterType<MessageService>().As<IMessageService>().WithParameter("uow", new IdentityUnitOfWork()).SingleInstance();

			builder.RegisterType<MessageManager>().As<IMessageManager>().WithParameter("db", new ApplicationContext()).InstancePerLifetimeScope();

			return builder;
		}
	}
}