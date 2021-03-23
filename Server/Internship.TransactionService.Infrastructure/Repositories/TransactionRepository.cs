using System;
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
        private const string ConnectionStringName = "DefaultConnection";

        public TransactionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<TransactionModel>> GetAll()
        {
            var sqlExpressionToGetAllTransactions = @"SELECT t.id as Id
                     , t.debtor_first_name       as DebtorFirstName
                     , t.debtor_last_name        as DebtorLastName
                     , t.debtor_account_number   as DebtorAccountNumber
                     , t.debtor_bank_id          as DebtorBankId
                     , t.creditor_first_name     as CreditorFirstName
                     , t.creditor_last_name      as CreditorLastName
                     , t.creditor_account_number as CreditorAccountNumber
                     , t.creditor_bank_id        as CreditorBankId
                     , t.transaction_id          as TransactionId
                     , t.incoming                as Incoming
                     , t.amount                  as Amount
                     , tStatus.status            as Status
                     , tStatus.dateStatusChanged as DateStatusChanged
                  FROM transactionservice_db.transactions as t
                  left join (
                    select row_number() over (partition by ts_i.transaction_id
                                order by ts_i.dateStatusChanged desc) as rOrder
                         , ts_i.transaction_id
                         , ts_i.status
                         , ts_i.dateStatusChanged
                      from transactionservice_db.transactionStatus as ts_i
                      ) as tStatus on tStatus.transaction_id = t.id
                                  and tStatus.rOrder = 1;";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString(ConnectionStringName));
            connection.Open();

            var transactions =
                await connection.QueryAsync<TransactionModel>(sqlExpressionToGetAllTransactions);

            return transactions;
        }

        public async Task<TransactionModel> GetById(int id)
        {
            var sqlExpressionToGetTransactionById = @"SELECT t.id as Id
                 , t.debtor_first_name       as DebtorFirstName
                 , t.debtor_last_name        as DebtorLastName
                 , t.debtor_account_number   as DebtorAccountNumber
                 , t.debtor_bank_id          as DebtorBankId
                 , t.creditor_first_name     as CreditorFirstName
                 , t.creditor_last_name      as CreditorLastName
                 , t.creditor_account_number as CreditorAccountNumber
                 , t.creditor_bank_id        as CreditorBankId
                 , t.transaction_id          as TransactionId
                 , t.incoming                as Incoming
                 , t.amount                  as Amount
                 , tStatus.status            as Status
                 , tStatus.dateStatusChanged as DateStatusChanged
              FROM transactionservice_db.transactions as t
              left join (
                select row_number() over (partition by ts_i.transaction_id
                            order by ts_i.dateStatusChanged desc) as rOrder
                     , ts_i.transaction_id
                     , ts_i.status
                     , ts_i.dateStatusChanged
                  from transactionservice_db.transactionStatus as ts_i
                  ) as tStatus on tStatus.transaction_id = t.id
                              and tStatus.rOrder = 1
                              where t.id = @id;";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString(ConnectionStringName));
            connection.Open();

            var transactions =
                await connection.QuerySingleAsync<TransactionModel>(sqlExpressionToGetTransactionById,
                    new
                    {
                        id
                    });

            return transactions;
        }

        public async Task<int> Add(TransactionModel transactionModel)
        {
            var sqlExpressionToInsert = @"INSERT INTO `transactionservice_db`.`transactions`
                                            (`debtor_first_name`, `debtor_last_name`, `debtor_account_number`, `debtor_bank_id`,
                                            `creditor_first_name`, `creditor_last_name`, `creditor_account_number`, `creditor_bank_id`,
                                            `incoming`, `transaction_id`, `amount`)
                                            VALUES
                                            (@debtor_first_name, @debtor_last_name, @debtor_account_number, @debtor_bank_id,
                                            @creditor_first_name, @creditor_last_name, @creditor_account_number, @creditor_bank_id,
                                            @incoming, @transaction_id, @amount);";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString(ConnectionStringName));
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

                incoming = transactionModel.Incoming,
                transaction_id = transactionModel.TransactionId,
                amount = transactionModel.Amount
            }); ;

            return await GetTransactionPrimaryKeyByTransactionId(transactionModel.TransactionId);
        }

        public async Task<int> UpdateStatus(TransactionStatusModel transactionStatusModel)
        {
            var sqlExpressionToInsert = @"INSERT INTO `transactionservice_db`.`transactionStatus`
	                                        (`status`, `reason`, `dateStatusChanged`, `transaction_id`)
	                                        VALUES
	                                        (@status, @reason, @date, @transaction_id);";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString(ConnectionStringName));
            connection.Open();

            var inserted = await connection.ExecuteAsync(sqlExpressionToInsert, new
            {
                transaction_id = transactionStatusModel.TransactionId,
                status = transactionStatusModel.Status,
                reason = transactionStatusModel.Reason,
                date = transactionStatusModel.DateStatusChanged,
            });

            return inserted;
        }

        public async Task<TransactionStatusModel> GetStatusByTransactionId(int transaction_id)
        {
            var sqlExpressionToGetStatusTransactionById = @"SELECT tStatus.id as Id
	                          , t.id         as TransactionId
                              , tStatus.status            as Status
                              , tStatus.reason            as Reason
                              , tStatus.dateStatusChanged as DateStatusChanged
                            FROM transactionservice_db.transactions as t
                            left join (
		                        select row_number() over (partition by ts_i.transaction_id
				                        order by ts_i.dateStatusChanged desc) as rOrder
											 , ts_i.id
                                             , ts_i.transaction_id
                                             , ts_i.status
                                             , ts_i.reason
                                             , ts_i.dateStatusChanged
				                        from transactionservice_db.transactionStatus as ts_i
				                        ) as tStatus on tStatus.transaction_id = t.id
                                                      and tStatus.rOrder = 1
                                                      where t.id = @id;";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString(ConnectionStringName));
            connection.Open();

            var transaction =
                await connection.QuerySingleAsync<TransactionStatusModel>(sqlExpressionToGetStatusTransactionById,
                    new
                    {
                        id = transaction_id
                    });

            return transaction;
        }

        public async Task<int> GetTransactionPrimaryKeyByTransactionId(Guid transaction_id)
        {
            var sqlExpressionToGetTransactionPrimaryKeyByTransactionId = @"SELECT t.id as Id
                  FROM transactionservice_db.transactions as t
                  WHERE transaction_id = @id;";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString(ConnectionStringName));
            connection.Open();

            var transaction =
                await connection.QuerySingleAsync<int>(sqlExpressionToGetTransactionPrimaryKeyByTransactionId,
                    new
                    {
                        id = transaction_id
                    });

            return transaction;
        }
    }
}