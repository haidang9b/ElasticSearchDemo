using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MessageBus.RabbitMQ;
public class RabbitMQMessageSender<T> : IMessageSender<T> where T : class
{
    private readonly IConnection _connection;

    private readonly ILogger<RabbitMQMessageSender<T>> _logger;

    private readonly QueueConfiguration _queueConfiguration;

    public RabbitMQMessageSender(
        IConnection connection,
        QueueConfiguration queueConfiguration,
        ILogger<RabbitMQMessageSender<T>> logger
    )
    {
        _connection = connection;
        _logger = logger;
        _queueConfiguration = queueConfiguration;
    }

    public async Task SendMessageAsync(BaseMessage<T> message)
    {
        await Task.Run(() =>
        {
            try
            {
                using (var channel = _connection.CreateModel())
                {
                    // Declare the queue with the configuration
                    channel.QueueDeclare(queue: _queueConfiguration.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var jsonMessage = JsonSerializer.Serialize(message);
                    var body = Encoding.UTF8.GetBytes(jsonMessage);

                    channel.BasicPublish(exchange: _queueConfiguration.ExchangeName, routingKey: _queueConfiguration.QueueName, basicProperties: null, body: body);

                    _logger.LogInformation($"Sent message to queue {_queueConfiguration.QueueName}: {jsonMessage}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while sending message to queue {_queueConfiguration.QueueName}");
                throw;
            }
        });
    }

    public async Task SendMessagesAsync(IEnumerable<BaseMessage<T>> messages)
    {
        await Task.Run(() =>
        {
            try
            {
                using (var channel = _connection.CreateModel())
                {
                    foreach (var message in messages)
                    {
                        // Declare the queue with the configuration
                        channel.QueueDeclare(queue: _queueConfiguration.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                        var jsonMessage = JsonSerializer.Serialize(message);
                        var body = Encoding.UTF8.GetBytes(jsonMessage);

                        channel.BasicPublish(exchange: _queueConfiguration.ExchangeName, routingKey: _queueConfiguration.QueueName, basicProperties: null, body: body);

                        _logger.LogInformation($"Sent message to queue {_queueConfiguration.QueueName}: {jsonMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while sending messages to queue {_queueConfiguration.QueueName}");
                throw;
            }
        });
    }
}
