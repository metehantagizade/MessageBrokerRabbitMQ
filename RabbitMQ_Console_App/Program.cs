using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Console_App
{
    public class Program
    {
        public static AMQP_Producer.RabbitMqService service;
        static void Main(string[] args)
        {
            CreateRabbitConnection(1);
        }

        public static void CreateRabbitConnection(int userId)
        {
            service = new AMQP_Producer.RabbitMqService(userId.ToString());
        }

    }
}
