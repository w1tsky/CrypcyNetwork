using Crypcy.ApplicationCore;
using Crypcy.NodeUI.Services;
using Microsoft.Extensions.Logging;
using Autofac;

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
            builder.Services.AddSingleton(provider =>
            {
                NodeUiService _ui = new NodeUiService();
                TcpNetwork _network = new TcpNetwork();
                Node _nodes = new Node(_network, _ui);
                
                return _ui;
            });

            return builder.Build();
        }
    }
}