using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQP_Common
{
    // To send Data to Message Broker create abject of this class fill it and send to Server
    public class ClientMessage
    {
        public string userId { get; set; }
        public Client_MessageType MessageType { get; set; }
        public object Message_Content { get; set; }
    }
}
