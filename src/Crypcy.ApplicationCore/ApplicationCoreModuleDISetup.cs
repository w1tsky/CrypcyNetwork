using Autofac;
using Crypcy.ApplicationCore.MessageProcessing;

namespace Crypcy.ApplicationCore
{
	public class ApplicationCoreModuleDISetup : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<MessageSender>().As<IMessageSender>().SingleInstance();
			builder.RegisterType<Node>().AsSelf().SingleInstance();
			builder.RegisterType<MessageHandler>().AsSelf();
		}
	}
}
