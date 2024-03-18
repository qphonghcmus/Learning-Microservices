
namespace OrderService;

public interface IOrderDeletor
{
    Task Delete(int orderId);
}