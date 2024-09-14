using DataReaderService.Messages;
using MessageBus.RabbitMQ;

namespace DataReaderService.HostedServices
{
    public class RabbitMQHostedService : IHostedService
    {
        private readonly IEnumerable<RabbitMQConsumer<TransactionMessage>> _consumers;

        public RabbitMQHostedService(IEnumerable<RabbitMQConsumer<TransactionMessage>> consumers)
        {
            _consumers = consumers;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var consumer in _consumers)
            {
                consumer.ConnectAndConsume();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Dispose of any consumers if necessary
            return Task.CompletedTask;
        }
    }
}
