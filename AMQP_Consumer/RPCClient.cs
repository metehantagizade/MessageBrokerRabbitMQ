using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMQP_Common;
using Newtonsoft.Json;

namespace AMQP_Consumer
{
    public class RPCClient
    {
        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private QueueingBasicConsumer consumer;

        public string userId;

        public RPCClient(string _userId)
        {

            this.userId = _userId;
            Broker broker = new Broker();
            connection = broker.GetRabbitMqConnection();
            if (connection != null)
            {
                channel = connection.CreateModel();




                replyQueueName = channel.QueueDeclare().QueueName;
                consumer = new QueueingBasicConsumer(channel);


                channel.BasicConsume(queue: replyQueueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
        }

        public Response_Message Call(ClientMessage message)
        {
            if (string.IsNullOrEmpty(this.userId) || channel == null)
                return null;

            var corrId = Guid.NewGuid().ToString();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId;

            DateTime start = DateTime.Now;

            
            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.QueueDeclare(queue: string.Format("{0}_{1}", Broker.que_name, userId), durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(0, 1, false);

            channel.BasicPublish(exchange: "",
                                 routingKey: string.Format("{0}_{1}", Broker.que_name, userId),
                                 basicProperties: props,
                                 body: messageBytes);



            int i = 0;

            while (true)
            {
                try
                {
                    BasicDeliverEventArgs ea = null;
                    bool result = consumer.Queue.Dequeue(Broker.timeout, out ea);
                    if (ea != null && ea.BasicProperties.CorrelationId == corrId)
                    {
                        var msg = Encoding.UTF8.GetString(ea.Body);

                        Response_Message obj = null;
                        try
                        {
                            obj = JsonConvert.DeserializeObject<Response_Message>(msg);

                            obj.request_start = start;
                            obj.request_end = DateTime.Now;
                            TimeSpan span = obj.request_end - obj.request_start;
                            obj.Response_Time = (int)span.TotalMilliseconds;
                        }
                        catch
                        {
                            obj = new Response_Message();
                        }
                        return obj;
                    }
                    else
                    {
                        return new Response_Message();
                    }
                }
                catch (System.IO.EndOfStreamException ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
                catch (TimeoutException ex)
                {
                    Console.WriteLine(ex);
                }

            }
        }
        public void Close()
        {
            if (connection != null)
                connection.Close();
        }
    }
}
