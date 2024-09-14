namespace MessageBus;

public interface IMessageSender<T> where T : class
{
    Task SendMessagesAsync(IEnumerable<BaseMessage<T>> messages);

    Task SendMessageAsync(BaseMessage<T> message);
}
