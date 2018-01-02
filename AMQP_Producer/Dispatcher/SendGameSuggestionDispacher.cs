using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMQP_Common;

namespace AMQP_Producer.Dispatcher
{
    public class SendGameSuggestionDispacher : ClientMessageDispatcher
    {
        public override Response_Message DispatchMessage(ClientMessage message)
        {
            LocalEntities.Type.GameAssignmentModel gameAssingment = JsonConvert.DeserializeObject<LocalEntities.Type.GameAssignmentModel>(message.Message_Content.ToString());
            return new Response_Message();
        }
    }
}
