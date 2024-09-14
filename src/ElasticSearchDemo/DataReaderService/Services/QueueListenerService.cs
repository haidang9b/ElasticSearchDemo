
using DataReaderService.Messages;
using MessageBus;

namespace DataReaderService.Services;

public class QueueListenerService : BackgroundService
{
    private readonly IMessageConsumer<TransactionMessage> _consumer;

    private readonly IReaderService _readerService;

    public QueueListenerService(IMessageConsumer<TransactionMessage> consumer, IReaderService readerService)
    {
        _consumer = consumer;
        _readerService = readerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Start listening for messages
        _consumer.StartListening();

        await Task.CompletedTask;
    }
}
