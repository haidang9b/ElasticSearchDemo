using Domain.Dto;

namespace ESD.SearchService.Services.Contracts;

public interface ITransactionService
{
    Task<List<TransactionDto>> SearchAsync(string query);

    Task IndexAsync();
}
