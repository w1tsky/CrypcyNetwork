using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Crypcy.Communication.Network
{
	public class TcpNetworkModuleDISetup : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<TcpNetwork>().AsImplementedInterfaces().InstancePerLifetimeScope();
		}
	}
}
