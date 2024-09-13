using SearchService.Services.Contracts;

namespace SearchService.Routes;

public static class TransactionEndpoints
{
    public static WebApplication MapTransactionEndpoins(this WebApplication app)
    {
        var group = app.MapGroup("/transactions");

        group.MapPost("/sync-data", async (ICsvService csvService) =>
        {
            await csvService.ReadTransactionAsync();

            return Results.Ok();
        });

        group.MapPost("/index-data", async (ITransactionService transactionService) =>
        {
            await transactionService.IndexAsync();

            return Results.Ok();
        });


        group.MapGet("/search", async (string query, ITransactionService transactionService) =>
        {
            var result = await transactionService.SearchAsync(query);

            return Results.Ok(result);
        });

        return app;
    }
}
