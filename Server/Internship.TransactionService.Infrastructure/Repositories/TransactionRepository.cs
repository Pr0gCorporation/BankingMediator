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
                        `transactions`.`amount` as Amount,
                        tstatus.status as Status,
                        tstatus.dateStatusChanged as DateStatusChanged
                    FROM `transactionservice_db`.`transactions` transactions
                    LEFT JOIN 
                    (
	                    -- select all the needed info about statuses
	                    SELECT tstatus.status as status, tstatus.transaction_id, tstatus.dateStatusChanged
			                    FROM `transactionservice_db`.`transactionStatus` tstatus 
                                -- join the status column with the transaction_id column with the maximum date
                                INNER JOIN
                                (
                                -- select transaction_id column with the maximum date
				                    SELECT tstatus.transaction_id, MAX(tstatus.dateStatusChanged) AS dateStatusChanged
					                     FROM `transactionservice_db`.`transactionStatus` tstatus 
					                     GROUP BY tstatus.transaction_id
                                ) groupedtstatus
                                ON tstatus.transaction_id = groupedtstatus.transaction_id
                                AND tstatus.dateStatusChanged = groupedtstatus.dateStatusChanged
	                    -- this querry selects all the transaction statuses, where each transaction status is selected only with the latest date
                    ) tstatus
                    ON transactions.transaction_id = tstatus.transaction_id;";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var transactions =
                await connection.QueryAsync<TransactionModel>(sqlExpressionToGetAllTransactions);

            return transactions;
        }

        public async Task<TransactionModel> GetById(int id)
        {
            var sqlExpressionToGetTransactionById = @"SELECT `transactions`.`id` as Id,
                            `transactions`.`debtor_first_name` as DebtorFirstName,
                            `transactions`.`debtor_last_name` as DebtorLastName,
                            `transactions`.`debtor_account_number` as DebtorAccountNumber,
                            `transactions`.`debtor_bank_id` as DebtorBankId,
                            `transactions`.`creditor_first_name` as CreditorFirstName,
                            `transactions`.`creditor_last_name` as CreditorLastName,
                            `transactions`.`creditor_account_number` as CreditorAccountNumber,
                            `transactions`.`creditor_bank_id` as CreditorBankId,
                            `transactions`.`transaction_id` as TransactionId,
                            `transactions`.`amount` as Amount,
	                        tstatus.status as Status,
	                        tstatus.dateStatusChanged as DateStatusChanged
                        FROM `transactionservice_db`.`transactions`
                        LEFT JOIN 
                        (
                            -- select all the needed info about statuses
                            SELECT tstatus.status as status, tstatus.transaction_id, tstatus.dateStatusChanged
                                    FROM `transactionservice_db`.`transactionStatus` tstatus 
                                    -- join the status column with the transaction_id column with the maximum date
                                    INNER JOIN
                                    (
                                    -- select transaction_id column with the maximum date
                                        SELECT tstatus.transaction_id, MAX(tstatus.dateStatusChanged) AS dateStatusChanged
                                                FROM `transactionservice_db`.`transactionStatus` tstatus 
                                                GROUP BY tstatus.transaction_id
                                    ) groupedtstatus
                                    ON tstatus.transaction_id = groupedtstatus.transaction_id
                                    AND tstatus.dateStatusChanged = groupedtstatus.dateStatusChanged
                            -- this querry selects all the transaction statuses, where each transaction status is selected only with the latest date
                        ) tstatus
                        ON transactions.transaction_id = tstatus.transaction_id
                        WHERE id = @id;";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var transactions =
                await connection.QuerySingleAsync<TransactionModel>(sqlExpressionToGetTransactionById,
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

        public async Task<int> UpdateStatus(TransactionStatusModel transactionStatusModel)
        {
            var sqlExpressionToInsert = @"INSERT INTO `transactionservice_db`.`transactionStatus`
	                                        (`status`, `reason`, `dateStatusChanged`, `transaction_id`)
	                                        VALUES
	                                        (@status, @reason, @date, @transaction_id);";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            var inserted = await connection.ExecuteAsync(sqlExpressionToInsert, new
            {
                transaction_id = transactionStatusModel.TransactionId,
                status = transactionStatusModel.Status,
                reason = transactionStatusModel.Reason,
                date = transactionStatusModel.Date,
            });

            return inserted;
        }
    }
}