using Dapper;
using System.Data.SqlClient;

namespace Ecomm.DataAccess;

public class InventoryUpdator(string connectionString) : IInventoryUpdator
{
    public async Task Update(int productId, int quantity)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.ExecuteAsync("UPDATE_INVENTORY", new { productId, quantity }, commandType: System.Data.CommandType.StoredProcedure);
    }
}
