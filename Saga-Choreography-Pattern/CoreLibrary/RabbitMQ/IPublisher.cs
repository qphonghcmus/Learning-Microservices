namespace CoreLibrary.RabbitMQ;

public interface IPublisher : IDisposable
{
    void Publish(string message, string routingKey, IDictionary<string, object> messageAttributes, string timeToLive = null);
}