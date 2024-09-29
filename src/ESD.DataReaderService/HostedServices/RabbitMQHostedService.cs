using ESD.DataReaderService.Messages;
using ESD.MessageBus.RabbitMQ;

namespace ESD.DataReaderService.HostedServices;

public class RabbitMQHostedService : BackgroundService
{
    private readonly IEnumerable<RabbitMQConsumer<TransactionMessage>> _consumers;

    private readonly IEnumerable<RabbitMQBulkConsumer<TransactionMessage>> _bulkConsumers;

    public RabbitMQHostedService(
        IEnumerable<RabbitMQConsumer<TransactionMessage>> consumers,
        IEnumerable<RabbitMQBulkConsumer<TransactionMessage>> bulkConsumers
    )
    {
        _consumers = consumers;
        _bulkConsumers = bulkConsumers;
    }

    //public async Task StartAsync(CancellationToken cancellationToken)
    //{
    //    foreach (var consumer in _consumers)
    //    {
    //        consumer.ConnectAndConsume();
    //    }

    //    foreach (var consumer in _bulkConsumers)
    //    {
    //        await consumer.ConnectAndConsumeAsync();
    //    }
    //}

    //public Task StopAsync(CancellationToken cancellationToken)
    //{
    //    // Dispose of any consumers if necessary
    //    return Task.CompletedTask;
    //}

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var consumer in _bulkConsumers)
        {
            await consumer.ConnectAndConsumeAsync();
        }
        foreach (var consumer in _consumers)
        {
            consumer.ConnectAndConsume();
        }
    }
}
