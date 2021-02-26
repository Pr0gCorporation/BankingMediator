using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Internship.FileService.Domain.Models;

namespace Internship.FileService.Service.DBAccess
{
    public class InsertTransactionToDb
    {
        public async Task Insert(SqlConnection connection, TransactionModel transaction, string fileName, byte[] file)
        {
            await using var sqlConnection = connection;

            var sqlExpressionInsertTransaction = $"INSERT INTO Payments (date, debtor, creditor, file_name, file) " +
                                                 $"VALUES ('{transaction.Date}'," + " @From, @To, @FileName, @File)";
            
            await using var insertTransaction = new SqlCommand(sqlExpressionInsertTransaction, sqlConnection);
            
            insertTransaction.Parameters.Add("@From", SqlDbType.NVarChar, 50).Value = transaction.Debtor;
            insertTransaction.Parameters.Add("@To", SqlDbType.NVarChar, 50).Value = transaction.Creditor;
            insertTransaction.Parameters.Add("@FileName", SqlDbType.NVarChar, 50).Value = fileName;
            insertTransaction.Parameters.Add("@File", SqlDbType.VarBinary, 350).Value = file;

            await sqlConnection.OpenAsync();
            await insertTransaction.ExecuteNonQueryAsync();
        }
    }
}