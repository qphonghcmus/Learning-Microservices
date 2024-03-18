using Ecomm.Models;

namespace Ecomm.DataAccess;
public interface IOrderDetailsProvider
{
    Task<OrderDetail[]> Get();
}