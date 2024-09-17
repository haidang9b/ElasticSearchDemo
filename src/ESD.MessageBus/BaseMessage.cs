namespace ESD.MessageBus;

public class BaseMessage<T> where T : class
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public T Data { get; set; }

    public static BaseMessage<T> Create(T Data)
    {
        return new BaseMessage<T>
        {
            Data = Data
        };
    }
}
