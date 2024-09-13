using Domain.Dto;

namespace SearchService.Services.Contracts;

public interface ITransactionService
{
    Task<List<TransactionDto>> SearchAsync(string query);

    Task IndexAsync();
}
