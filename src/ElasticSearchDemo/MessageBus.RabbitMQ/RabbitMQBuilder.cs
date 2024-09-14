using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MessageBus.RabbitMQ
{
    public class RabbitMQBuilder
    {
        private readonly IServiceCollection _services;

        public RabbitMQBuilder(IServiceCollection services)
        {
            _services = services;
        }

        #region Consumer Methods

        public RabbitMQBuilder AddConsumer<TMessage, TConsumer>(QueueConfiguration queueConfiguration)
            where TMessage : class
            where TConsumer : class, IMessageConsumer<TMessage>
        {
            _services.AddSingleton<IMessageConsumer<TMessage>, TConsumer>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                var logger = sp.GetRequiredService<ILogger<TConsumer>>();

                return (TConsumer)Activator.CreateInstance(typeof(TConsumer), connection, queueConfiguration, logger);
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
}
