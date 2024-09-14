using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MessageBus.RabbitMQ;

public abstract class RabbitMQMessageConsumer<T> : IMessageConsumer<T> where T : class
{
    private readonly IConnection _connection;

    private readonly QueueConfiguration _queueConfiguration;

    private readonly ILogger<RabbitMQMessageConsumer<T>> _logger;

    private IModel _channel;

    public RabbitMQMessageConsumer(
        IConnection connection,
        QueueConfiguration queueConfiguration,
        ILogger<RabbitMQMessageConsumer<T>> logger)
    {
        _connection = connection;
        _queueConfiguration = queueConfiguration;
        _logger = logger;

        CreateChannel();
    }

    private void CreateChannel()
    {
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _queueConfiguration.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        _logger.LogInformation($"Declared queue: {_queueConfiguration.QueueName}");
    }

    public void StartListening()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<BaseMessage<T>>(messageJson);

            _logger.LogInformation($"Received message from queue {_queueConfiguration.QueueName}: {messageJson}");

            try
            {
                await ConsumeAsync(message);
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing message");

                throw;
                // You can handle failed message consumption here (e.g. by sending it to a dead-letter queue)
            }
        };

        _channel.BasicConsume(queue: _queueConfiguration.QueueName, autoAck: false, consumer: consumer);
        _logger.LogInformation($"Started listening on queue {_queueConfiguration.QueueName}");
    }

    public abstract Task ConsumeAsync(BaseMessage<T> message);
    

    public void StopListening()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
