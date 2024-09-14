using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MessageBus.RabbitMQ;

public class RabbitMQConsumer<TMessage> : IDisposable where TMessage : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly QueueConfiguration _queueConfig;
    private IModel _channel;

    public RabbitMQConsumer(IServiceProvider serviceProvider, IConnection connection, QueueConfiguration queueConfig)
    {
        _serviceProvider = serviceProvider;
        _connection = connection;
        _queueConfig = queueConfig;
    }

    public void ConnectAndConsume()
    {
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _queueConfig.QueueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            await ConsumeAsync(body);
        };

        _channel.BasicConsume(queue: _queueConfig.QueueName, autoAck: true, consumer: consumer);
    }

    private async Task ConsumeAsync(byte[] messageBody)
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetService<IMessageHandler<TMessage>>();

        if (handler != null)
        {
            var jsonString = Encoding.UTF8.GetString(messageBody);

            var message = JsonSerializer.Deserialize<BaseMessage<TMessage>>(messageBody);
            await handler.HandleAsync(message);
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
