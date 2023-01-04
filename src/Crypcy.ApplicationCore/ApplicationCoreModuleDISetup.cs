using Autofac;
using Crypcy.ApplicationCore.TempMessageSolution;
using Crypcy.ApplicationCore.TempMessageSolution.MessageHandles;
using Crypcy.ApplicationCore.TempMessageSolution.MessagesTypes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore
{
    public class ApplicationCoreModuleDISetup : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<MessageSender>().AsSelf().InstancePerLifetimeScope();
			builder.RegisterType<Nodes>().AsSelf().InstancePerLifetimeScope();
			builder.RegisterType<FrontDataHandler>().AsSelf().InstancePerLifetimeScope();
			builder.RegisterAssemblyTypes(typeof(ApplicationCoreModuleDISetup).Assembly)
				.Where(t => !t.IsAbstract && (t.GetInterface(nameof(IMessageHandler)) is not null))
				.As<IMessageHandler>()
				.InstancePerLifetimeScope();
		}
	}
}
