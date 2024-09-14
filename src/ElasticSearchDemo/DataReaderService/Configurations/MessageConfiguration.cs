using MessageBus;

namespace DataReaderService.Configurations;

public class MessageConfiguration
{
    public required QueueConfiguration TransactionQueue { get; set; }
}
