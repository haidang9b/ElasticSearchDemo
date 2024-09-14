using Domain.Dto;

namespace DataReaderService.Services;

public interface IReaderService
{
    Task IndexDocumentsAsyns(params TransactionDto[] transactions);
}
