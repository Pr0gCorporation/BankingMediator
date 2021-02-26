using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Internship.FileService.Domain.Models;

namespace Internship.FileService.Service.DBAccess
{
    public class InsertTransactionToDb
    {
        public async Task Insert(SqlConnection connection, TransactionModel transaction)
        {
            await using var sqlConnection = connection;

            var sqlExpressionInsertTransaction = $"INSERT INTO Transactions (Id, Date, [From], [To], FileName) " +
                                                 $"VALUES ('{transaction.TransactionId}', '{transaction.Date}'," +
                                                 " @From, @To, @FileName)";
            
            await using var insertTransaction = new SqlCommand(sqlExpressionInsertTransaction, sqlConnection);
            
            insertTransaction.Parameters.Add("@From", SqlDbType.NVarChar, 50).Value = transaction.Creditor;
            insertTransaction.Parameters.Add("@To", SqlDbType.NVarChar, 50).Value = transaction.Debtor;
            insertTransaction.Parameters.Add("@FileName", SqlDbType.NVarChar, 50).Value = transaction.FileName;

            await sqlConnection.OpenAsync();
            await insertTransaction.ExecuteNonQueryAsync();
        }
    }
}