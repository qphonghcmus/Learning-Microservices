using RabbitMQ.Client;

namespace CoreLibrary.RabbitMQ;
public interface IConnectionProvider : IDisposable
{
    IConnection GetConnection();
}