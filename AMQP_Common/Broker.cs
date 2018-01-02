using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMQP_Common
{
    public class Broker
    {
        // Inside message broker set name for Queues
        public static string que_name = "projectName_Data";//Queues Name
        public static int timeout = 10000; // Set timeout

        // Create connection function for message broker connection
        public IConnection GetRabbitMqConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();

            connectionFactory.HostName = ""; //Set Ip address of message broker
            connectionFactory.UserName = ""; // define username and password on Message Broker Server
            connectionFactory.Password = ""; // write password of message broker account
            //connectionFactory.Port = 15672; // İf Server has port then set port in this line (default port for web panel is 15672)
            try
            {
                return connectionFactory.CreateConnection();  // 
            }
            catch (BrokerUnreachableException ex)
            {
                return null;
            }

        }
    }
}
