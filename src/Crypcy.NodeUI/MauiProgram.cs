using Crypcy.ApplicationCore;
using Crypcy.NodeUI.Services;
using Microsoft.Extensions.Logging;
using Crypcy.Communication.Network;
using Crypcy.ApplicationCore.MessageProcessing;

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
                MessageSender _messageSender = new MessageSender(_network);

                Node _nodes = new Node(_network, _messageSender, _ui);

                return _ui;
            });


            return builder.Build();
        }

      
    }
}







