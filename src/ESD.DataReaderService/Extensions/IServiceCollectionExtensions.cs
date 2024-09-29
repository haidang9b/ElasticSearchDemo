using ESD.DataReaderService.Configurations;
using ESD.DataReaderService.Consumers;
using ESD.DataReaderService.HostedServices;
using ESD.DataReaderService.Messages;
using ESD.DataReaderService.Services;
using ESD.MessageBus.RabbitMQ;
using ESD.SearchEngine.Extensions;

namespace ESD.DataReaderService.Extensions;
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
        services.AddSingleton(sp =>
            RabbitMQConnectionFactory.CreateConnection(rabbitMQConfig));

        var messageConfig = configuration.GetSection("MessageConfiguration").Get<MessageConfiguration>()
            ?? throw new ArgumentNullException(nameof(MessageConfiguration));

        var rabbitMQBuilder = new RabbitMQBuilder(services);

        rabbitMQBuilder
            .AddBulkConsumer<TransactionMessage, TransactionConsumer>(messageConfig.TransactionQueue);

        services.AddHostedService<RabbitMQHostedService>();
        return services;
    }
}