namespace MessageBus;

public interface IMessageConsumer<T> where T : class
{
    Task ConsumeAsync(BaseMessage<T> message);

    void StopListening();

    void StartListening();
}
