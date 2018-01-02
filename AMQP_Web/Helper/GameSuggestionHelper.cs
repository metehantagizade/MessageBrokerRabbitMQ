using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMQP_Common;
namespace AMQP_Web.Helper
{
    public class GameSuggestionHelper
    {
        public static void SendGameSuggestionListAsync(string userId, Web_ProjectName_DAL.Model.GameAssignmentModel gameAssignmentModel)
        {
            Thread _thread = new Thread(() => SendGameSuggestionList(userId, gameAssignmentModel));
            _thread.Start();
        }

        public static void SendGameSuggestionList(string userId, Web_ProjectName_DAL.Model.GameAssignmentModel gameAssignmentModel)
        {
            
            AMQP_Consumer.RPCClient client = new AMQP_Consumer.RPCClient(userId);
            ClientMessage message = new ClientMessage();
            message.userId = userId;
            message.MessageType = Client_MessageType.sendGameSuggestion;
            message.Message_Content = gameAssignmentModel;
            client.Call(message);
            client.Close();
        }
    }
}
