using AMQP_Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMQP_Producer
{
    public delegate void AMQP_Event_Handler(AMQP_EventArgs e);
    public delegate void AMQP_Event_Handler_Start();
    public class AMQP_EventArgs : EventArgs
    {
        public Client_MessageType type { get; set; }
    }
    public class RabbitMqService
    {
        public event AMQP_Event_Handler AMQP_Request;
        public event AMQP_Event_Handler_Start AMQP_Request_Start;
        public string userId;
        public Thread _thread;
        private System.Timers.Timer checkStatusTimer;
        AMQP_Common.Broker broker;

        IConnection connection;
        IModel channel;
        EventingBasicConsumer consumer;

        public RabbitMqService()
        {
            broker = new Broker();

            _thread = new Thread(new ThreadStart(Remote_Process_Call));
            _thread.Start();
        }

        private void CheckStatusTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReConnect();
        }

        public RabbitMqService(string _userId)
        {
            broker = new Broker();
            this.userId = _userId;
            _thread = new Thread(new ThreadStart(Remote_Process_Call));
            _thread.Start();

            checkStatusTimer = new System.Timers.Timer(60000);
            checkStatusTimer.Elapsed += CheckStatusTimer_Elapsed;
            checkStatusTimer.Start();

        }

        public bool IsConnectionOpen()
        {
            return connection != null ? connection.IsOpen : false;
        }

        public void Close()
        {
            try
            {
                connection.Close();
                StopListener();

            }
            catch
            {

            }
            finally
            {
                connection = null;
            }
        }

        public void ReConnect()
        {

            //Close();
            //Remote_Process_Call();

            if ((connection != null && !connection.IsOpen))
            {
                Remote_Process_Call();
            }
            else if (connection == null)
            {
                Remote_Process_Call();
            }
        }

        public void Remote_Process_Call()
        {
            using (connection = broker.GetRabbitMqConnection())
                if (connection != null)
                {
                    using (channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: string.Format("{0}_{1}", Broker.que_name, userId), durable: false, exclusive: false, autoDelete: false, arguments: null);
                        channel.BasicQos(0, 1, false);
                        consumer = new EventingBasicConsumer(channel);
                        channel.BasicConsume(queue: string.Format("{0}_{1}", Broker.que_name, userId), autoAck: true, consumer: consumer);
                        Console.WriteLine(string.Format(" [x] Awaiting RPC requests for {0}_{1}", Broker.que_name, userId));

                        consumer.Received += (model, ea) =>
                        {
                            string response = null;

                            var body = ea.Body;
                            var props = ea.BasicProperties;
                            var replyProps = channel.CreateBasicProperties();
                            replyProps.CorrelationId = props.CorrelationId;

                            try
                            {
                                try
                                {
                                    //RaiseEvent_Start();
                                }
                                catch(Exception ex)
                                {

                                }

                                var msg = Encoding.UTF8.GetString(body);
                                ClientMessage message = null;
                                try
                                {
                                    //In web application before send data to broker we can serialize data as json in ClientMessage format, inside this function we can deserialize recived data to ClientMessage object
                                    message = Newtonsoft.Json.JsonConvert.DeserializeObject<ClientMessage>(msg);
                                    //message.userId = userId;
                                    //message.MessageType = Client_MessageType.sendGameSuggestion;
                                    //message.Message_Content = msg;
                                }
                                catch(Exception ex)
                                {
                                    throw new Exception("unknown message format");
                                }

                                ClientMessageDispatcher dispatcher = AMQP_Common.Dispatcher.DispatcherHelper.GetDispatcher(message);
                                // Write data to database
                                Response_Message obj = dispatcher.DispatchMessage(message);
                                if (obj != null)
                                {
                                    Console.WriteLine(" [.] fib({0}) for {1}", message.userId, message.MessageType.ToString());
                                }
                                //RaiseEvent(message);
                                response = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" [.] " + e.Message);
                                response = "";
                            }
                            finally
                            {
                                var responseBytes = Encoding.UTF8.GetBytes(response);
                                //channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
                                //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                            }
                        };

                        //Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();

                        while (true)
                        {
                        }
                    }
                }
        }
        public void RaiseEvent(ClientMessage message)
        {
            AMQP_EventArgs args = new AMQP_EventArgs();
            args.type = message.MessageType;
            AMQP_Request(args);
        }
        public void RaiseEvent_Start()
        {
            AMQP_Request_Start();
        }

        public void StopListener()
        {
            if (_thread != null)
                _thread.Abort();
        }
    }
}
