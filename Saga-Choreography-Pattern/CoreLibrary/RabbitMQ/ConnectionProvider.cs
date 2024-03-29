﻿using RabbitMQ.Client;

namespace CoreLibrary.RabbitMQ;

public class ConnectionProvider : IConnectionProvider
{
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private bool _disposed;
    public ConnectionProvider(string uri)
    {
        _factory = new ConnectionFactory
        {
            Uri = new Uri(uri)
        };
        _connection = _factory.CreateConnection();
    }
    public IConnection GetConnection()
    {
        return _connection;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            _connection?.Close();

        _disposed = true;
    }
}
