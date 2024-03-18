using CoreLibrary.RabbitMQ;
using Ecomm.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController(
    IOrderDetailsProvider orderDetailsProvider, 
    IPublisher publisher, 
    IOrderCreator orderCreator) : ControllerBase
{

    // GET: api/<OrderController>
    [HttpGet]
    public IEnumerable<OrderDetail> Get()
    {
        return orderDetailsProvider.Get();
    }

    // GET api/<OrderController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<OrderController>
    [HttpPost]
    public async Task Post([FromBody] OrderDetail orderDetail)
    {
        var id = await orderCreator.Create(orderDetail);
        publisher.Publish(JsonConvert.SerializeObject(
            new OrderRequest
            {
                OrderId = id,
                Quantity = orderDetail.Quantity,
                ProductId = orderDetail.ProductId
            }
            ), "order.created", null);
    }

    // PUT api/<OrderController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<OrderController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}