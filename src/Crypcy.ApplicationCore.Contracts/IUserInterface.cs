using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.Contracts
{
	public interface IUserInterface
	{
        event Action<int> OnStartNode;
        event Action<CancellationToken> OnStopNode;
        event Action<string, int> OnConnectToNodeRequest;
        event Action<string> OnCreateGroupRequest;
        event Action<string, string> OnSendMessageRequest;
		
		void ShowMessage(string node, string message);
        void ShowGroup(string group);
        void NodeConnectedNotification(string node);
		void NodeDiconnectedNotification(string node);
	}
}
