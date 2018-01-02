using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMQP_Producer.Dispatcher;

namespace AMQP_Common.Dispatcher
{
    public class DispatcherHelper
    {
        public static ClientMessageDispatcher GetDispatcher(ClientMessage message)
        {
            switch (message.MessageType)
            {
                case Client_MessageType.sendGameSuggestion:
                    return new SendGameSuggestionDispacher();
                default:
                    return null;
            }
        }
    }
}
