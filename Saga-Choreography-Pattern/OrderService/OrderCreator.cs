using Dapper;
using System.Data.SqlClient;

namespace OrderService;

public class OrderCreator(
    string connectionString,
    ILogger<OrderCreator> logger
    ) : IOrderCreator
{
    public async Task<int> Create(OrderDetail orderDetail)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var transaction = await connection.BeginTransactionAsync();
        try
        {
            var id = await connection.QuerySingleAsync<int>("CREATE_ORDER", new { userId = 1, userName = orderDetail.User }, transaction: transaction, commandType: System.Data.CommandType.StoredProcedure);
            await connection.ExecuteAsync("CREATE_ORDER_DETAILS",
                new { orderId = id, productId = orderDetail.ProductId, quantity = orderDetail.Quantity, productName = orderDetail.Name }
                , transaction: transaction, commandType: System.Data.CommandType.StoredProcedure);
            transaction.Commit();
            return id;
        }
        catch(Exception ex)
        {
            logger.LogError($"Error: {ex}");
            transaction.Rollback();
            return -1;
        }
    }
}

