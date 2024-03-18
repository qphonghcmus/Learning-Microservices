
using CoreLibrary.RabbitMQ;
using Ecomm.DataAccess;
using Ecomm.Models;
using Newtonsoft.Json;

namespace Ecomm;

public class OrderCreatedListener(
        IPublisher publisher,
        ISubscriber subscriber,
        IInventoryUpdator inventoryUpdator
    ) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        subscriber.Subscribe(Subscribe);
        return Task.CompletedTask;
    }

    private bool Subscribe(string message, IDictionary<string, object> header)
    {
        var response = JsonConvert.DeserializeObject<OrderRequest>(message);
        try
        {
            inventoryUpdator.Update(response.ProductId, response.Quantity).GetAwaiter().GetResult();
            publisher.Publish(JsonConvert.SerializeObject(
                new InventoryResponse
                {
                    OrderId = response.OrderId,
                    IsSuccess = true
                }
                ), "inventory.response", null);
        }
        catch (Exception)
        {
            publisher.Publish(JsonConvert.SerializeObject(
                new InventoryResponse
                {
                    OrderId = response.OrderId,
                    IsSuccess = false
                }
                ), "inventory.response", null);
        }
        return true;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

