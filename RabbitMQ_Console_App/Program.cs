using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Console_App
{
    public class Program
    {
        public static AMQP_Producer.RabbitMqService service = new AMQP_Producer.RabbitMqService("1");


        public static void Main(string[] args)
        {
            
        }
        
        public void CreateRabbitConnection()
        {
            service.AMQP_Request_Start += RabbitMqService_AMQP_Request_Start;
            service.AMQP_Request += RabbitMqService_AMQP_Request;
        }
        private void RabbitMqService_AMQP_Request(AMQP_Producer.AMQP_EventArgs e)
        {
            //this.SuggetionList.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => CreateSuggestionCard()));
        }
        private void RabbitMqService_AMQP_Request_Start()
        {
            //this.SuggetionList.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => SetSynchronizeInfoToVisible()));
        }

    }
}
