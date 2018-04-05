using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum MessageType_ENUM
{
    Standard,
    Warning,
    Error,
    Exception,
    Indifferent,
    DEFAULT_WRITE_ALL,
    Event,
    ________
}

namespace DAS_SDK.MVC.Model.Debug
{
    class Message<T>
    {
        public T MessageContent;
        public MessageType_ENUM _MessageType;

        public Message(T messageContent, MessageType_ENUM messageType = MessageType_ENUM.Standard)
        {
            this.MessageContent = messageContent;
            this._MessageType = messageType;
        }

    }
}
