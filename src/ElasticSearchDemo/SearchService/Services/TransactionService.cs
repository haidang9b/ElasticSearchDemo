using Domain.Dto;
using Domain.Models;
using MessageBus;
using Microsoft.EntityFrameworkCore;
using SearchEngine.Services.Constracts;
using SearchService.Data;
using SearchService.Messages;
using SearchService.Services.Contracts;

namespace SearchService.Services;

public class TransactionService : ITransactionService
{
    private readonly IElasticsearchService _elasticsearchService;

    private readonly SearchDbContext _dbContext;

    private readonly IMessageSender<TransactionMessage> _messageSender;

    public TransactionService(
        IElasticsearchService elasticsearchService,
        SearchDbContext dbContext,
        IMessageSender<TransactionMessage> messageSender
    )
    {
        _elasticsearchService = elasticsearchService;
        _dbContext = dbContext;
        _messageSender = messageSender;
    }

    public async Task IndexAsync()
    {
        var transactions = await _dbContext.Transactions.Select(x => new TransactionMessage
        {
            Amount = x.Amount,
            Code = x.Code,
            CreatedDate = x.CreatedDate,
            Id = x.Id,
            Note = x.Note,
        }).ToListAsync();

        var messages = transactions.Select(BaseMessage<TransactionMessage>.Create);

        await _messageSender.SendMessagesAsync(messages);

        await _elasticsearchService.IndexDocumentsAsync(transactions, nameof(Transaction).ToLower());
    }

    public async Task<List<TransactionDto>> SearchAsync(string query)
    {
        var result = await _elasticsearchService.SearchDocumentsAsync<TransactionDto>(nameof(Transaction).ToLower(), query);

        return result.Documents.ToList();
    }
}
