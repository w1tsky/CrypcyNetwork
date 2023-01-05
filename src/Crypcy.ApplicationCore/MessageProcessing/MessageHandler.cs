using Crypcy.ApplicationCore.Contracts.Network;
using System.Text.Json;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Crypcy.ApplicationCore.Contracts;
using Crypcy.ApplicationCore.MessagesTypes;

namespace Crypcy.ApplicationCore.MessageProcessing
{
    public class MessageHandler
    {
        private readonly IReciveData _reciveData;
        private readonly IUserInterface _userInterface;

        public MessageHandler(IReciveData reciveData, IUserInterface userInterface)
        {
            _reciveData = reciveData;
            _userInterface = userInterface;

            _reciveData.OnNewMessageRecived += ReciveData;
        }

        private Type GetMessageType(MessageKind messageKind)
        {
            var MessageKindTypeMap = new ReadOnlyDictionary<MessageKind, Type>(
                new Dictionary<MessageKind, Type>()
                {
                    {MessageKind.Text, typeof(TextMessage)}
                });

            return MessageKindTypeMap.GetValueOrDefault(messageKind)
                ?? throw new Exception($"Not found MessageType for {messageKind}");
        }

        private void ReciveData(string node, byte[] data)
        {
            var messageKind = JsonSerializer.Deserialize<BaseMessage>(data)!.MessageType;
            var messageType = GetMessageType(messageKind);
            dynamic message = JsonSerializer.Deserialize(data, messageType)!;

            Handle(message, node);
        }

        #region Messages handlers:
        private void Handle(TextMessage textMessage, string node)
        {
            _userInterface.ShowMessage(node, textMessage.Text);
        }
        #endregion
    }
}
