using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Internship.TransactionService.Application.Repository.TransactionRepository;
using Internship.TransactionService.Domain.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Internship.TransactionService.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IConfiguration _configuration;

        public TransactionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<TransactionModel>> GetAll()
        {
            var sqlExpressionToGetAllTransactions = $"SELECT * FROM `transactionservice_db`.`transactions`";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();
            
            var transactions = await connection.QueryAsync<TransactionModel>(sqlExpressionToGetAllTransactions, connection);
            return transactions;
        }

        public async Task Add(TransactionModel entity)
        {
            // var sqlExpressionToInsertTransaction = "INSERT INTO `fileservice_db`.`files`"+
            //                                        "(`date`) VALUES (@Date);";
            //
        }
    }
}