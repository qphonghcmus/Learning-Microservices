using Dapper;
using System.Data.SqlClient;

namespace OrderService;

public class OrderDeletor(string connectionString) : IOrderDeletor
{
    public async Task Delete(int orderId)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            await connection.ExecuteAsync("DELETE FROM OrderDetail WHERE OrderId = @orderId", new { orderId }, transaction: transaction);
            await connection.ExecuteAsync("DELETE FROM [Order] WHERE Id = @orderId", new { orderId }, transaction: transaction);
            transaction.Commit();
        }
        catch(Exception ex)
        {
            transaction.Rollback();
        }
    }
}
