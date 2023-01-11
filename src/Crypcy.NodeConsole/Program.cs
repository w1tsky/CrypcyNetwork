using Autofac;
using Crypcy.ApplicationCore;
using Crypcy.ApplicationCore.Contracts;
using Crypcy.ApplicationCore.MessageProcessing;
using Crypcy.Communication.Network;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace Crypcy.NodeConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());

            builder.Sources.Clear();
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();

            configuration.GetReloadToken().RegisterChangeCallback(state =>
            {
                Console.WriteLine("Restarting console application due to configuration change...");
                Environment.Exit(0);
            }, null);


            // DI:
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new ApplicationCoreModuleDISetup());
            containerBuilder.RegisterModule(new TcpNetworkModuleDISetup());
            containerBuilder.RegisterType<ConsoleImp>().As<IUserInterface>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterInstance(configuration).As<IConfigurationRoot>().SingleInstance();
            var container = containerBuilder.Build();
            // end DI

            using var scope = container.BeginLifetimeScope(b => b.RegisterBuildCallback(c =>
            {
                c.Resolve<Node>();
                c.Resolve<MessageHandler>();
            }));
            var console = scope.Resolve<ConsoleImp>();

            CancellationTokenSource cts = new CancellationTokenSource();


            if (!configuration.GetValue<bool>("Node:RunAsService"))
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    Console.WriteLine("Type help for list commands");

                    try
                    {
                        console.NewInput(Console.ReadLine()!, cts);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Oops: {ex.Message}");
                    }
                }
            }
            else
            {
                console.NewInput($"start:{configuration.GetValue<int>("Node:Port")}", cts);
                while (true)
                {
                    await Task.Delay(1000);
                }
            }
        }



    }
}