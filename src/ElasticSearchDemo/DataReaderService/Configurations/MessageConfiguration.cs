using ESD.MessageBus;

namespace ESD.DataReaderService.Configurations;

public class MessageConfiguration
{
    public required QueueConfiguration TransactionQueue { get; set; }
}
