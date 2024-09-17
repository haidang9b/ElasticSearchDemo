namespace MessageBus;

public interface IMessageHandler<T> where T : class
{
    Task HandleAsync(BaseMessage<T> message);
}
