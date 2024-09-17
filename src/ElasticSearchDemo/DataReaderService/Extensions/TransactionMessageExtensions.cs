using ESD.DataReaderService.Messages;
using ESD.Domain.Dto;

namespace ESD.DataReaderService.Extensions;

public static class TransactionMessageExtensions
{
    public static TransactionDto ToDto(this TransactionMessage message)
    {
        return new TransactionDto
        {
            Amount = message.Amount,
            Code = message.Code,
            CreatedDate = message.CreatedDate,
            Id = message.Id,
            Note = message.Note,
        };
    }
}
