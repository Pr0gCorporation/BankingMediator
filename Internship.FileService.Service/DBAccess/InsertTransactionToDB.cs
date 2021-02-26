using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Internship.FileService.Domain.Models;

namespace Internship.FileService.Service.DBAccess
{
    public class InsertTransactionToDb
    {
        public static async Task Insert(SqlConnection connection, TransactionModel transaction)
        {
            await using var sqlConnection = connection;

            var sqlExpressionInsertTransaction = $"INSERT INTO Payments (date, type, file_name, file) " +
                                                 $"VALUES ('{transaction.Date}'," + " @Type, @FileName, @File)";
            
            await using var insertTransaction = new SqlCommand(sqlExpressionInsertTransaction, sqlConnection);
            
            insertTransaction.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = transaction.Type;
            insertTransaction.Parameters.Add("@FileName", SqlDbType.NVarChar, 50).Value = transaction.FileName;
            insertTransaction.Parameters.Add("@File", SqlDbType.VarBinary, 350).Value = transaction.File;

            await sqlConnection.OpenAsync();
            await insertTransaction.ExecuteNonQueryAsync();
        }
    }
}