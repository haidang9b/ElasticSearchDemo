using Domain.Dto;
using SearchEngine.Services.Constracts;

namespace DataReaderService.Services;

public class ReaderService : IReaderService
{
    private readonly IElasticsearchService _elasticsearchService;

    public ReaderService(IElasticsearchService elasticsearchService)
    {
        _elasticsearchService = elasticsearchService;
    }

    public async Task IndexDocumentsAsyns(params TransactionDto[] transactions)
    {
        await _elasticsearchService.IndexDocumentsAsync(transactions, "transactions");
    }
}
