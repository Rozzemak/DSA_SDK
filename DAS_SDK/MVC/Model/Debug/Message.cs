using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAS_SDK.MVC.Model.Debug
{
    public enum MessageTypeEnum
    {
        Standard,
        Warning,
        Error,
        Exception,
        Indifferent,
        DefaultWriteAll,
        Event,
        ________
    }

    class Message<T>
    {
        public T MessageContent;
        public MessageTypeEnum MessageType;

        public Message(T messageContent, MessageTypeEnum messageType = MessageTypeEnum.Standard)
        {
            this.MessageContent = messageContent;
            this.MessageType = messageType;
        }

    }
}