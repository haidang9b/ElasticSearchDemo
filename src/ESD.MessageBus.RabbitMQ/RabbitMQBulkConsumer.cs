using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ESD.MessageBus.RabbitMQ;

public class RabbitMQBulkConsumer<TMessage> : IDisposable where TMessage : class
{
    private const int PrefetchCountDefault = 100;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly QueueConfiguration _queueConfig;
    private IModel _channel;

    public RabbitMQBulkConsumer(IServiceProvider serviceProvider, IConnection connection, QueueConfiguration queueConfig)
    {
        _serviceProvider = serviceProvider;
        _connection = connection;
        _queueConfig = queueConfig;
    }

    public async Task ConnectAndConsumeAsync()
    {
        _channel = _connection.CreateModel();

        int prefetchCount = _queueConfig.Prefetch ?? PrefetchCountDefault; // Number of messages to fetch at a time

        _channel.BasicQos(prefetchSize: 0, prefetchCount: (ushort)prefetchCount, global: false);

        _channel.QueueDeclare(queue: _queueConfig.QueueName,
                           durable: true,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);

        var consumer = new EventingBasicConsumer(_channel);

        List<BaseMessage<TMessage>> messageBatch = new();

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<BaseMessage<TMessage>>(body);

                messageBatch.Add(message);
            }
            catch (Exception) { }

            // Acknowledge the message
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            if (messageBatch.Count >= prefetchCount)
            {
                await ProcessBatchAsync(messageBatch);
                messageBatch.Clear();  // Clear batch after processing
            }
        };

        _channel.BasicConsume(queue: _queueConfig.QueueName, autoAck: false, consumer: consumer);
    }

    private async Task ProcessBatchAsync(List<BaseMessage<TMessage>> messageBatch)
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetService<IBulkMessageHandler<TMessage>>();
        if (handler != null)
        {
            await handler.HandleAsync(messageBatch);
        }
        else
        {
            Console.WriteLine($"No handler registered for message type: {typeof(TMessage).Name}");
        }
    }

    public void Dispose()
    {
        _channel?.Close();
    }
}
