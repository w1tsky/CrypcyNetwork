using Crypcy.Desktop.Views;
using Crypcy.Network.PeerItems;
using Crypcy.Network.PeerNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Crypcy.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Peer LocalPeer;

        public delegate void LocalPeertHandler(Peer peer);

        public event LocalPeertHandler LocalPeerUpdated;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void OnLocalPeerUpdated(Peer peer)
        {
            LocalPeer = peer;
            LocalPeer.PeerPacketReceived += Peer_OnPeerPacketUpdate;
            PeerLocalEndpoint.Text = LocalPeer.LocalEndpoint.ToString();

            LocalPeerUpdated?.Invoke(peer);
        }

        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void MinimizeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void MaximizeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Maximized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnStartPeer_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StartPeer());
        }

        private void Peer_OnPeerPacketUpdate(Packet packet)
        {
            Dispatcher.Invoke(delegate
            {
                PeerConsoleBox.Text += packet.ToString() + '\n';
                PeerConsoleBox.CaretIndex = PeerConsoleBox.Text.Length;
                PeerConsoleBox.ScrollToEnd();
            });

        }

        private void Peer_OnResultsUpdate(object sender, string e)
        {
            Dispatcher.Invoke(delegate
            {
                PeerConsoleBox.Text += e + '\n';
                PeerConsoleBox.CaretIndex = PeerConsoleBox.Text.Length;
                PeerConsoleBox.ScrollToEnd();
            });

        }
    }
}
