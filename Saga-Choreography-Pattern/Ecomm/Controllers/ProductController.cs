using CoreLibrary.RabbitMQ;
using Ecomm.DataAccess;
using Ecomm.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecomm.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductProvider _productProvider;
    private readonly IPublisher _publisher;

    public ProductController(IProductProvider productProvider, IPublisher publisher)
    {
        _productProvider = productProvider;
        _publisher = publisher;
    }

    // GET: api/<ProductController>
    [HttpGet]
    public IEnumerable<Product> Get()
    {
        return _productProvider.Get();
    }

    // GET api/<ProductController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<ProductController>
    [HttpPost]
    public void Post([FromBody] Product product)
    {
        _publisher.Publish(JsonConvert.SerializeObject(product), "report.product", null);
    }

    // PUT api/<ProductController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ProductController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}