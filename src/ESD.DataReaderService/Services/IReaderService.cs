using ESD.Domain.Dto;

namespace ESD.DataReaderService.Services;

public interface IReaderService
{
    Task IndexDocumentsAsyns(params TransactionDto[] transactions);
}
