namespace AMQP_Common
{
    public abstract class ClientMessageDispatcher
    {
        public abstract Response_Message DispatchMessage(ClientMessage message);
    }
}
