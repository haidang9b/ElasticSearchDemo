using MessageBus;

namespace SearchService.Configurations;

public class MessageConfiguration
{
    public required QueueConfiguration TransactionQueue { get; set; }
}
