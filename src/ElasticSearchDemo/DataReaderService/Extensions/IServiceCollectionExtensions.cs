using DataReaderService.Configurations;
using DataReaderService.Consumers;
using DataReaderService.HostedServices;
using DataReaderService.Messages;
using DataReaderService.Services;
using MessageBus.RabbitMQ;
using RabbitMQ.Client;
using SearchEngine.Extensions;

namespace DataReaderService.Extensions;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddServices();

        services.AddElasticsearch(configuration);

        services.AddRabbitMQ(configuration);

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IReaderService, ReaderService>();

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

        rabbitMQBuilder
            .AddConsumer<TransactionMessage, TransactionConsumer>(messageConfig.TransactionQueue);

        services.AddHostedService<RabbitMQHostedService>();
        return services;
    }
}