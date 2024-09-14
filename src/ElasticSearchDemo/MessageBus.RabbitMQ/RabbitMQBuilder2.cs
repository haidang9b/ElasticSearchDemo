/*using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MessageBus.RabbitMQ
{
    public class RabbitMQBuilder2
    {
        private readonly IServiceCollection _services;

        private readonly Dictionary<Type, QueueConfiguration> _queueRegisters = new Dictionary<Type, QueueConfiguration>();
        
        private readonly Dictionary<Type, Type> _consumerRegisters = new Dictionary<Type, Type>();

        public RabbitMQBuilder2(IServiceCollection services)
        {
            _services = services;
        }

        #region Consumer Methods

        public RabbitMQBuilder2 AddConsumer<TMessage, TConsumer>(QueueConfiguration queueConfiguration)
            where TMessage : class
            where TConsumer : class, IMessageConsumer<TMessage>
        {
            *//*_services.AddSingleton<IMessageConsumer<TMessage>, TConsumer>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                var logger = sp.GetRequiredService<ILogger<TConsumer>>();

                return (TConsumer)Activator.CreateInstance(typeof(TConsumer), connection, queueConfiguration, logger);
            });*//*

            var consumerType = typeof(RabbitMQMessageConsumer<>).MakeGenericType(typeof(TMessage));

            _consumerRegisters.Add(consumerType, typeof(TMessage));

            return this;
        }

        public RabbitMQBuilder2 AddConsumerService<TMessage, TConsumerService>()
            where TMessage : class
            where TConsumerService : BackgroundService
        {
            _services.AddHostedService<TConsumerService>();
            return this;
        }

        #endregion

        #region Sender Methods

        public RabbitMQBuilder2 AddSender<TMessage>(QueueConfiguration queueConfiguration)
            where TMessage : class
        {
            *//*_services.AddSingleton<IMessageSender<TMessage>, RabbitMQMessageSender<TMessage>>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                var logger = sp.GetRequiredService<ILogger<RabbitMQMessageSender<TMessage>>>();

                return new RabbitMQMessageSender<TMessage>(connection, queueConfiguration, logger);
            });*//*

            _queueRegisters.Add(typeof(TMessage), queueConfiguration);

            return this;
        }

        public IServiceCollection Build()
        {

        }

        #endregion
    }
}
*/