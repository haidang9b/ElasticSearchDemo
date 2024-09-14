using RabbitMQ.Client;

namespace MessageBus.RabbitMQ;

public class RabbitMQConnectionFactory
{
    public static IConnection CreateConnection(RabbitMQConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration.HostName,
            UserName = configuration.Username,
            Password = configuration.Password
        };

        return factory.CreateConnection();
    }
}
