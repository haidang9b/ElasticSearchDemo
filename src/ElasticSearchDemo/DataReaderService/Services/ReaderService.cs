using ESD.Domain.Dto;
using ESD.SearchEngine.Services.Constracts;

namespace ESD.DataReaderService.Services;

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
