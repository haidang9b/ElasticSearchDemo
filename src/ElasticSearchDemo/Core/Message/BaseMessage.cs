namespace Core.Message;

public abstract class BaseMessage <T> where T : class
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset MessageCreated { get; set; } = DateTimeOffset.Now;

    public required T Data { get; set; }
}
