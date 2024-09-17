using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace ESD.MessageBus.RabbitMQ;

public class RabbitMQBuilder
{
    private readonly IServiceCollection _services;

    public RabbitMQBuilder(IServiceCollection services)
    {
        _services = services;
    }

    #region Consumer Methods

    public RabbitMQBuilder AddConsumer<TMessage, TMessageHandler>(QueueConfiguration queueConfig)
        where TMessage : class
        where TMessageHandler : class, IMessageHandler<TMessage>
    {
        _services.AddScoped<IMessageHandler<TMessage>, TMessageHandler>();

        // Register the consumer in DI
        _services.AddSingleton(serviceProvider =>
        {
            var connection = serviceProvider.GetRequiredService<IConnection>();
            return new RabbitMQConsumer<TMessage>(serviceProvider, connection, queueConfig);
        });

        return this;
    }

    public RabbitMQBuilder AddConsumerService<TMessage, TConsumerService>()
        where TMessage : class
        where TConsumerService : BackgroundService
    {
        _services.AddHostedService<TConsumerService>();
        return this;
    }

    #endregion

    #region Sender Methods

    public RabbitMQBuilder AddSender<TMessage>(QueueConfiguration queueConfiguration)
        where TMessage : class
    {
        _services.AddSingleton<IMessageSender<TMessage>, RabbitMQMessageSender<TMessage>>(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            var logger = sp.GetRequiredService<ILogger<RabbitMQMessageSender<TMessage>>>();

            return new RabbitMQMessageSender<TMessage>(connection, queueConfiguration, logger);
        });

        return this;
    }

    #endregion
}
