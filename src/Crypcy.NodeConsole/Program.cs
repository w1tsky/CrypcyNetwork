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
        static void Main(string[] args)
        {

            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());

            builder.Sources.Clear();
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            if (args.Length > 0)
            {
                builder.AddCommandLine(args);
            }
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

            Console.WriteLine("Type help for list commands");

            CancellationTokenSource cts = new CancellationTokenSource();
            // Start a thread to read input from the console

            while (!cts.Token.IsCancellationRequested)
            {
                string input = Console.ReadLine();

                try
                {
                    console.NewInput(input!, cts);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Oops: {ex.Message}");
                }

            }


        }


    }
}