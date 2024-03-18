using Ecomm.DataAccess;
using Ecomm.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ecomm.Controllers;
public class HomeController : ControllerBase
{
    private readonly IOrderDetailsProvider _orderDetailsProvider;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IOrderDetailsProvider orderDetailsProvider, ILogger<HomeController> logger)
    {
        _orderDetailsProvider = orderDetailsProvider;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var orderDetails = await _orderDetailsProvider.Get();
        return Ok(orderDetails);
    }

    public IActionResult Privacy()
    {
        return Ok();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return Ok(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}