using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace Crypcy.NodeUI.Services
{
    public class NodeEventsService
    {
        private readonly NodeUiService _nodeUiService;
        private readonly Subject<string> _nodeConnectedSubject = new Subject<string>();
        private readonly Subject<string> _nodeDisconnectedSubject = new Subject<string>();
        private readonly Subject<(string, string)> _messageReceivedSubject = new Subject<(string, string)>();

        public NodeEventsService(NodeUiService nodeUiService)
        {
            _nodeUiService = nodeUiService;
            _nodeUiService.OnNodeConnected += OnNodeConnected;
            _nodeUiService.OnNodeDisconnected += OnNodeDisconnected;
            _nodeUiService.OnMessageReceived += OnMessageReceived;
        }

        public IObservable<string> NodeConnectedObservable => _nodeConnectedSubject.AsObservable();
        public IObservable<string> NodeDisconnectedObservable => _nodeDisconnectedSubject.AsObservable();
        public IObservable<(string, string)> MessageReceivedObservable => _messageReceivedSubject.AsObservable();

        private void OnNodeConnected(string node)
        {
            _nodeConnectedSubject.OnNext(node);
        }

        private void OnNodeDisconnected(string node)
        {
            _nodeDisconnectedSubject.OnNext(node);
        }

        private void OnMessageReceived(string node, string message)
        {
            _messageReceivedSubject.OnNext((node, message));
        }
    }

}
