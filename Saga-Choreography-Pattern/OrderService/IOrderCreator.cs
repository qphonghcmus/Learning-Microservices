
namespace OrderService;

public interface IOrderCreator
{
    Task<int> Create(OrderDetail orderDetail);
}