﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crypcy.ApplicationCore.Contracts
{
    public interface IUserInterface
    {
        public event Action<string, string> OnSendMessageRequest;
        public event Action<int> OnStartNode;


        void ShowMessage(string node, string message);

        void NodeConnectedNotification(string node);

        void NodeDiconnectedNotification(string node);
    }
}
