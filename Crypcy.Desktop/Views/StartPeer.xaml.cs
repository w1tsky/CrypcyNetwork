using Crypcy.Network.PeerItems;
using Crypcy.Network.PeerNetwork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crypcy.Desktop.Views
{
    /// <summary>
    /// Interaction logic for StartPeer.xaml
    /// </summary>
    public partial class StartPeer : UserControl
    {
        Peer LocalPeer;

        private IPEndPoint localEndpoint;
        public IPEndPoint LocalEndpoint
        {
            get { return localEndpoint; }
        }

        public StartPeer()
        {
            InitializeComponent();

        }

        private void StartPeer_Btn(object sender, RoutedEventArgs e)
        {

            localEndpoint = new IPEndPoint(address: IPAddress.Any, port: int.Parse(PortBox.Text));

            LocalPeer = new Peer(localEndpoint);

            if (!LocalPeer.PeerStarted)
                LocalPeer.StartPeer();
            else
                MessageBox.Show("Peer Already Started");

            var mainWindow = (MainWindow)Window.GetWindow(this);

            mainWindow.OnLocalPeerUpdated(LocalPeer);
            

        }


        private void StopPeer_Btn(object sender, RoutedEventArgs e)
        {
            LocalPeer.StopPeer();

            var CurrentWindow = (MainWindow)Window.GetWindow(this);

            CurrentWindow.OnLocalPeerUpdated(LocalPeer);

        }
    }
}
