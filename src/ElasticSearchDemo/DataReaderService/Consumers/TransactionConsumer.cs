using DataReaderService.Extensions;
using DataReaderService.Messages;
using DataReaderService.Services;
using MessageBus;
using MessageBus.RabbitMQ;
using RabbitMQ.Client;

namespace DataReaderService.Consumers
{
    public class TransactionConsumer : RabbitMQMessageConsumer<TransactionMessage>
    {
        private readonly IReaderService _readerService;

        public TransactionConsumer(
            IConnection connection,
            QueueConfiguration queueConfiguration,
            ILogger<RabbitMQMessageConsumer<TransactionMessage>> logger,
            IReaderService readerService
        ) : base(connection, queueConfiguration, logger)
        {
            _readerService = readerService;
        }

        public override async Task ConsumeAsync(BaseMessage<TransactionMessage> message)
        {
            await _readerService.IndexDocumentsAsyns(message.Data.ToDto());
        }
    }
}
