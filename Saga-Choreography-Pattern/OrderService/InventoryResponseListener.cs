
using CoreLibrary.RabbitMQ;
using Ecomm.Models;
using Newtonsoft.Json;

namespace OrderService;

public class InventoryResponseListener(ISubscriber subscriber,
    IOrderDeletor orderDeletor) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        subscriber.Subscribe(Subscribe);
        return Task.CompletedTask;
    }

    private bool Subscribe(string message, IDictionary<string, object> header)
    {
        var response = JsonConvert.DeserializeObject<InventoryResponse>(message);
        if (!response.IsSuccess)
        {
            orderDeletor.Delete(response.OrderId).GetAwaiter().GetResult();
        }
        return true;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
