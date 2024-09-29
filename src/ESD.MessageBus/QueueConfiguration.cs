namespace ESD.MessageBus;

public class QueueConfiguration
{
    public required string QueueName { get; set; }

    public string ExchangeName { get; set; } = string.Empty; // Optional exchange name

    public int? Prefetch { get; set; } = 100;
}
