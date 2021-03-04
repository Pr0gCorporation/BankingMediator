using System;
using System.Collections.Generic;
using System.Linq;
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
            var sqlExpressionToGetAllTransactions = @"SELECT trans.id,
    trans.transaction_id as TransactionId,
    trans.amount as Amount,
    debtor.id as Id,
    debtor.first_name as FirstName,
    debtor.last_name as LastName,
    debtor.account_number as AccountNumber,
    debtor.bank_id as BankId,
    creditor.id as Id,
    creditor.first_name as FirstName,
    creditor.last_name as LastName,
    creditor.account_number as AccountNumber,
    creditor.bank_id as BankId
FROM transactionservice_db.transactions trans
left JOIN transactionservice_db.accounts debtor
ON (trans.debtor_id = debtor.id)
left JOIN transactionservice_db.accounts creditor
ON (trans.creditor_id = creditor.id);";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var transactions =
                await connection.QueryAsync<TransactionModel, AccountModel, AccountModel, TransactionModel>(
                    sqlExpressionToGetAllTransactions,
                    (transaction, debtor, creditor) =>
                    {
                        transaction.Debtor = debtor;
                        transaction.Creditor = creditor;
                        return transaction;
                    },
                    splitOn: "Id,Id");

            return transactions;
        }
        
        public async Task<TransactionModel> GetByTransactionId(Guid transactionId)
        {
            var sqlExpressionToGetAllTransactions = @"SELECT trans.id,
    trans.transaction_id as TransactionId,
    trans.amount as Amount,
    debtor.id as Id,
    debtor.first_name as FirstName,
    debtor.last_name as LastName,
    debtor.account_number as AccountNumber,
    debtor.bank_id as BankId,
    creditor.id as Id,
    creditor.first_name as FirstName,
    creditor.last_name as LastName,
    creditor.account_number as AccountNumber,
    creditor.bank_id as BankId
FROM transactionservice_db.transactions trans
left JOIN transactionservice_db.accounts debtor
ON (trans.debtor_id = debtor.id)
left JOIN transactionservice_db.accounts creditor
ON (trans.creditor_id = creditor.id)
WHERE trans.transaction_id = @transId;";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var transactions =
                await connection.QueryAsync<TransactionModel, AccountModel, AccountModel, TransactionModel>(
                    sqlExpressionToGetAllTransactions,
                    (transaction, debtor, creditor) =>
                    {
                        transaction.Debtor = debtor;
                        transaction.Creditor = creditor;
                        return transaction;
                    },
                    new { transId = transactionId },
                    splitOn: "Id,Id");

            return transactions.FirstOrDefault();
        }

        public async Task Add(TransactionModel entity)
        {
            var sqlExpressionToInsert = @"INSERT INTO `transactionservice_db`.`transactions`
(`transaction_id`,
`debtor_id`,
`creditor_id`,
`amount`)
VALUES
(@Id,
@DID,
@CID,
@Amount);";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            await connection.ExecuteAsync(sqlExpressionToInsert, new
            {
                Id = entity.TransactionId,
                DID = entity.Debtor.Id,
                CID = entity.Creditor.Id,
                Amount = entity.Amount
            });
        }
    }
}