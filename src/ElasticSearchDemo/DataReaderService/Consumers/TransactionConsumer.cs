using ESD.DataReaderService.Extensions;
using ESD.DataReaderService.Messages;
using ESD.DataReaderService.Services;
using ESD.MessageBus;

namespace ESD.DataReaderService.Consumers;

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
