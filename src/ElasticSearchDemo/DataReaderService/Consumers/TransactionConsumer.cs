using DataReaderService.Extensions;
using DataReaderService.Messages;
using DataReaderService.Services;
using MessageBus;

namespace DataReaderService.Consumers
{
    public class TransactionConsumer : IMessageHandler<TransactionMessage>
    {
        private readonly IReaderService _readerService;

        public TransactionConsumer(IReaderService readerService)
        {
            _readerService = readerService;
        }

        public async Task HandleAsync(BaseMessage<TransactionMessage> message)
        {
            await _readerService.IndexDocumentsAsyns(message.Data.ToDto());
        }
    }
}
