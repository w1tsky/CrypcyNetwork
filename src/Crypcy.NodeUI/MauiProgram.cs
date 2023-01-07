using Crypcy.ApplicationCore;
using Crypcy.NodeUI.Services;
using Microsoft.Extensions.Logging;
using Autofac;
using Crypcy.ApplicationCore.Contracts;
using Crypcy.Communication.Network;

namespace Crypcy.NodeUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

         #if DEBUG
		    builder.Services.AddBlazorWebViewDeveloperTools();
		    builder.Logging.AddDebug();
#endif

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new ApplicationCoreModuleDISetup());
            containerBuilder.RegisterModule(new TcpNetworkModuleDISetup());
            containerBuilder.RegisterType<NodeUiService>().As<IUserInterface>().AsSelf().InstancePerLifetimeScope();
            var container = containerBuilder.Build();

            return builder.Build();
        }
    }
}