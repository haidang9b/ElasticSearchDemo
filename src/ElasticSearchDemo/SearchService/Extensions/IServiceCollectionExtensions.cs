using Microsoft.EntityFrameworkCore;
using SearchEngine.Extensions;
using SearchService.Data;
using SearchService.Services;
using SearchService.Services.Contracts;

namespace SearchService.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext(configuration);
        services.AddServices();

        services.AddElasticsearch(configuration);

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SearchConnection");

        services.AddDbContext<SearchDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICsvService, CsvService>();
        services.AddScoped<ITransactionService, TransactionService>();

        return services;
    }
}
