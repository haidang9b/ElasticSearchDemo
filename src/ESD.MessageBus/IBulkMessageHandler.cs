namespace ESD.MessageBus;

public interface IBulkMessageHandler<T> where T : class
{
    Task HandleAsync(IEnumerable<BaseMessage<T>> message);
}
