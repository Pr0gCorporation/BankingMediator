using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Internship.TransactionService.Domain.Interfaces;
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
            var sqlExpressionToGetAllTransactions = @"SELECT `transactions`.`id` as Id,
                                                        `transactions`.`debtor_first_name` as DebtorFirstName,
                                                        `transactions`.`debtor_last_name` as DebtorLastName,
                                                        `transactions`.`debtor_account_number` as DebtorAccountNumber,
                                                        `transactions`.`debtor_bank_id` as DebtorBankId,
                                                        `transactions`.`creditor_first_name` as CreditorFirstName,
                                                        `transactions`.`creditor_last_name` as CreditorLastName,
                                                        `transactions`.`creditor_account_number` as CreditorAccountNumber,
                                                        `transactions`.`creditor_bank_id` as CreditorBankId,
                                                        `transactions`.`transaction_id` as TransactionId,
                                                        `transactions`.`amount` as Amount
                                                    FROM `transactionservice_db`.`transactions`;";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var transactions =
                await connection.QueryAsync<TransactionModel>(sqlExpressionToGetAllTransactions);

            return transactions;
        }

        public async Task<TransactionModel> GetById(int id)
        {
            var sqlExpressionToGetAllTransactions = @"SELECT `transactions`.`id` as Id,
                                                        `transactions`.`debtor_first_name` as DebtorFirstName,
                                                        `transactions`.`debtor_last_name` as DebtorLastName,
                                                        `transactions`.`debtor_account_number` as DebtorAccountNumber,
                                                        `transactions`.`debtor_bank_id` as DebtorBankId,
                                                        `transactions`.`creditor_first_name` as CreditorFirstName,
                                                        `transactions`.`creditor_last_name` as CreditorLastName,
                                                        `transactions`.`creditor_account_number` as CreditorAccountNumber,
                                                        `transactions`.`creditor_bank_id` as CreditorBankId,
                                                        `transactions`.`transaction_id` as TransactionId,
                                                        `transactions`.`amount` as Amount
                                                    FROM `transactionservice_db`.`transactions`
                                                    WHERE id = @id;";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var transactions =
                await connection.QuerySingleAsync<TransactionModel>(sqlExpressionToGetAllTransactions,
                    new
                    {
                        id = id
                    });

            return transactions;
        }

        public async Task<int> Add(TransactionModel transactionModel)
        {
            var sqlExpressionToInsert = @"INSERT INTO `transactionservice_db`.`transactions`
                                            (`debtor_first_name`, `debtor_last_name`, `debtor_account_number`, `debtor_bank_id`,
                                            `creditor_first_name`, `creditor_last_name`, `creditor_account_number`, `creditor_bank_id`,
                                            `transaction_id`, `amount`)
                                            VALUES
                                            (@debtor_first_name, @debtor_last_name, @debtor_account_number, @debtor_bank_id,
                                            @creditor_first_name, @creditor_last_name, @creditor_account_number, @creditor_bank_id,
                                            @transaction_id, @amount);";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var inserted = await connection.ExecuteAsync(sqlExpressionToInsert, new
            {
                debtor_first_name = transactionModel.DebtorFirstName,
                debtor_last_name = transactionModel.DebtorLastName,
                debtor_account_number = transactionModel.DebtorAccountNumber,
                debtor_bank_id = transactionModel.DebtorBankId,

                creditor_first_name = transactionModel.CreditorFirstName,
                creditor_last_name = transactionModel.CreditorLastName,
                creditor_account_number = transactionModel.CreditorAccountNumber,
                creditor_bank_id = transactionModel.CreditorBankId,

                transaction_id = transactionModel.TransactionId,
                amount = transactionModel.Amount
            });

            return inserted;
        }
    }
}