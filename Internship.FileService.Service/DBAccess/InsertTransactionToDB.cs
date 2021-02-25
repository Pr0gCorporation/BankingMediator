using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Internship.FileService.Service.Consumers;
using Internship.FileService.Service.Models;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service.DBAccess
{
    public class InsertTransactionToDb
    {
        private readonly ILogger<TransactionConsumer> _logger;

        public InsertTransactionToDb(ILogger<TransactionConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Insert(SqlConnection connection, TransactionModel transaction)
        {
            await using var sqlConnection = connection;

            var sqlExpressionInsertTransaction = $"INSERT INTO Transactions (Id, Date, [From], [To], FileName) " +
                                                 $"VALUES ('{transaction.Id}', '{transaction.Date}'," +
                                                 " @From, @To, @FileName)";
            SqlCommand insertTransaction;

            await using (insertTransaction = new SqlCommand(sqlExpressionInsertTransaction, sqlConnection))
            {
                insertTransaction.Parameters.Add("@From", SqlDbType.NVarChar, 50).Value = transaction.From;
                insertTransaction.Parameters.Add("@To", SqlDbType.NVarChar, 50).Value = transaction.To;
                insertTransaction.Parameters.Add("@FileName", SqlDbType.NVarChar, 50).Value = transaction.FileName;
            }

            await sqlConnection.OpenAsync();

            try
            {
                _ = await insertTransaction.ExecuteNonQueryAsync();
                _logger.LogInformation($"Inserted to the {sqlConnection.Database} successfully!");
            }
            catch (SqlException sqlException)
            {
                if (sqlException.Message.Contains("Violation of PRIMARY KEY constraint"))
                    _logger.LogWarning($"Record already exists {transaction.Id}!");
                else 
                    _logger.LogCritical(sqlException.Message);
            }
            finally
            {
                await sqlConnection.CloseAsync();
            }
        }
    }
}