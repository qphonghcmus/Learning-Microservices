namespace Ecomm.Models;
public class OrderRequest
{
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
}