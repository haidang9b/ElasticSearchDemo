using MessageBus;

namespace ESD.SearchService.Configurations;

public class MessageConfiguration
{
    public required QueueConfiguration TransactionQueue { get; set; }
}
