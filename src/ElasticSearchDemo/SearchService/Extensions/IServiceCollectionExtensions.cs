using MessageBus.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using SearchEngine.Extensions;
using SearchService.Configurations;
using SearchService.Data;
using SearchService.Messages;
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

        services.AddRabbitMQ(configuration);

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SearchConnection");

        services.AddDbContext<SearchDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICsvService, CsvService>();
        services.AddScoped<ITransactionService, TransactionService>();

        return services;
    }

    private static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMQConfig = configuration.GetSection("RabbitMQConfiguration").Get<RabbitMQConfiguration>()
            ?? throw new ArgumentNullException(nameof(RabbitMQConfiguration));

        // Register the RabbitMQ connection as a singleton
        services.AddSingleton<IConnection>(sp =>
            RabbitMQConnectionFactory.CreateConnection(rabbitMQConfig));

        var messageConfig = configuration.GetSection("MessageConfiguration").Get<MessageConfiguration>()
            ?? throw new ArgumentNullException(nameof(MessageConfiguration));

        var rabbitMQBuilder = new RabbitMQBuilder(services);

        rabbitMQBuilder.AddSender<TransactionMessage>(messageConfig.TransactionQueue);


        return services;
    }
}
