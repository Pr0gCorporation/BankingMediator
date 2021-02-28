using System.Threading.Tasks;
using Internship.FileService.Domain.Models;
using MySql.Data.MySqlClient;

namespace Internship.FileService.Service.DBAccess
{
    public class InsertTransactionToDb
    {
        public async Task Insert(string connectionString, TransactionModel transaction)
        {
            await using var sqlConnection = new MySqlConnection(connectionString);

            var sqlExpressionInsertTransaction = "INSERT INTO `db`.`payments`"+
                                                 "(`date`, `type`, `filename`, `file`) VALUES (@Date, @Type, @FileName, @File);";

            await using var insertTransaction = new MySqlCommand(sqlExpressionInsertTransaction, sqlConnection);
            
            insertTransaction.Parameters.Add("@Date", MySqlDbType.DateTime, 50).Value = transaction.Date;
            insertTransaction.Parameters.Add("@Type", MySqlDbType.VarChar, 50).Value = transaction.Type;
            insertTransaction.Parameters.Add("@FileName", MySqlDbType.VarChar, 50).Value = transaction.FileName;
            insertTransaction.Parameters.Add("@File", MySqlDbType.Blob, 350).Value = transaction.File;

            await sqlConnection.OpenAsync();
            await insertTransaction.ExecuteNonQueryAsync();
        }
    }
}