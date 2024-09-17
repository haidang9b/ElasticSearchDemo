using CsvHelper;
using CsvHelper.Configuration;
using Domain.Models;
using ESD.SearchService.Data;
using ESD.SearchService.Maps;
using ESD.SearchService.Services.Contracts;
using System.Globalization;

namespace ESD.SearchService.Services;

public class CsvService(SearchDbContext dbContext) : ICsvService
{
    public async Task ReadTransactionAsync()
    {
        var filePath = "transactions.csv";

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.Context.RegisterClassMap<TransactionMap>();
            var transactions = csv.GetRecords<Transaction>().ToArray();

            if (transactions.Any())
            {
                await dbContext.AddRangeAsync(transactions);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
