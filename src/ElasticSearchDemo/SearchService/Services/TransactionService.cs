using Domain.Dto;
using Domain.Models;
using SearchEngine.Services.Constracts;
using SearchService.Data;
using SearchService.Services.Contracts;

namespace SearchService.Services;

public class TransactionService : ITransactionService
{
    private readonly IElasticsearchService _elasticsearchService;

    private readonly SearchDbContext _dbContext;

    public TransactionService(IElasticsearchService elasticsearchService, SearchDbContext dbContext)
    {
        _elasticsearchService = elasticsearchService;
        _dbContext = dbContext;
    }

    public async Task IndexAsync()
    {
        var transactions = _dbContext.Transactions.Select(x => new TransactionDto
        {
            Amount = x.Amount,
            Code = x.Code,
            CreatedDate = x.CreatedDate,
            Id = x.Id,
            Note = x.Note,
        });

        await _elasticsearchService.IndexDocumentsAsync(transactions, nameof(Transaction).ToLower());
    }

    public async Task<List<TransactionDto>> SearchAsync(string query)
    {
        var result = await _elasticsearchService.SearchDocumentsAsync<TransactionDto>(nameof(Transaction).ToLower(), query);

        return result.Documents.ToList();
    }
}
