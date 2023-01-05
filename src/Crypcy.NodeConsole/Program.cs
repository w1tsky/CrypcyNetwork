using Autofac;
using Crypcy.ApplicationCore;
using Crypcy.ApplicationCore.Contracts;
using Crypcy.ApplicationCore.MessageProcessing;
using Crypcy.Communication.Network;

namespace Crypcy.NodeConsole
{
    internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Short manual:");
			Console.WriteLine("Type \"start:PORT\" to start node with port(1024 to 49151).");
			Console.WriteLine("Type \"l\" to show list of nodes.");
			Console.WriteLine("For sending message you must write message in specific format:");
			Console.WriteLine("Template: Nmsg:your message");
			Console.WriteLine("N is number of target node, (you can get nodes via l command) your message is your message");
			Console.WriteLine("Server starting...");

			// DI:
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterModule(new ApplicationCoreModuleDISetup());
			containerBuilder.RegisterModule(new TcpNetworkModuleDISetup());
			containerBuilder.RegisterType<ConsoleImp>().As<IUserInterface>().AsSelf().InstancePerLifetimeScope();
			var container = containerBuilder.Build();
			// end DI

			using var scope = container.BeginLifetimeScope(b => b.RegisterBuildCallback(c =>
			{
				c.Resolve<Node>();
				c.Resolve<MessageHandler>();
			}));
			var console = scope.Resolve<ConsoleImp>();

			while (true)
			{
				try
				{
					console.NewInput(Console.ReadLine()!);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Oops: {ex.Message}");
				}
			}
		}
	}
}