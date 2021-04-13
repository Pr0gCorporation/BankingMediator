using Dapper;
using Internship.AccountService.Domain.Interfaces;
using Internship.AccountService.Domain.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internship.AccountService.Infrastructure.DAL
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;
        private const string ConnectionStringName = "DefaultConnection";

        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> GetAccountPrimaryKeyByIBAN(string IBAN)
        {
            var sqlExpressionToGetAccountPrimaryKeyByIBAN = @"
                SELECT id FROM accountservice_db.account
                WHERE IBAN = @IBAN;";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<int>(
                    sqlExpressionToGetAccountPrimaryKeyByIBAN, new
                    {
                        IBAN
                    });
            }
        }

        public async Task<int> GetCashbookPrimaryKeyByAccountId(int accountId)
        {
            var sqlExpressionToGetCashbookPrimaryKeyByAccountId = @"
                SELECT id FROM accountservice_db.cashbook
                WHERE accountid = @accountId;";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<int>(
                    sqlExpressionToGetCashbookPrimaryKeyByAccountId, new
                    {
                        accountId
                    });
            }
        }

        public async Task<decimal> GetSumOfCashbookRecordsByCashbookId(int cashbookId)
        {
            return ((await this.GetSentMoneyBeforeDate(cashbookId, DateTime.Now.Date.AddDays(1))) * -1)
                + await this.GetReceivedMoneyBeforeDate(cashbookId, DateTime.Now.Date.AddDays(1));
        }

        public async Task<decimal> GetSumOfCashbookRecordsByCashbookIdBeforeDate(int cashbookId, DateTime beforeDate)
        {
            return ((await this.GetSentMoneyBeforeDate(cashbookId, beforeDate)) * -1)
                + await this.GetReceivedMoneyBeforeDate(cashbookId, beforeDate);
        }

        private async Task<decimal> GetSentMoneyBeforeDate(int cashbookIdFrom, DateTime beforeDate)
        {
            var sqlExpressionToGetSentMoney = @"
                SELECT SUM(amount)
                FROM accountservice_db.cashbookRecords
                WHERE cashbookidFrom = @cashbookIdFrom
                AND date < @beforeDate;";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return await connection.QuerySingleAsync<decimal>(
                    sqlExpressionToGetSentMoney, new
                    {
                        cashbookIdFrom,
                        beforeDate
                    });
            }
        }

        private async Task<decimal> GetReceivedMoneyBeforeDate(int cashbookIdTo, DateTime beforeDate)
        {
            var sqlExpressionToGetSentMoney = @"
                SELECT SUM(amount)
                FROM accountservice_db.cashbookRecords
                WHERE cashbookidTo = @cashbookIdTo
                AND date < @beforeDate;";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return await connection.QuerySingleAsync<decimal>(
                    sqlExpressionToGetSentMoney, new
                    {
                        cashbookIdTo,
                        beforeDate
                    });
            }
        }

        public async Task<int> InsertCashbookRecord(CashbookRecordModel cashbookRecordModel)
        {
            var sqlExpressionToInsert = @"
                INSERT INTO `accountservice_db`.`cashbookRecords` 
                    (`cashbookidFrom`, `cashbookidTo`, `date`, `amount`, `original_reference`) VALUES 
                (@cashbookidFrom, @cashbookidTo, @date, @amount, @reference);";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();

                return await connection.ExecuteAsync(sqlExpressionToInsert, new
                {
                    cashbookidFrom = cashbookRecordModel.CashbookIdDebtor,
                    cashbookidTo = cashbookRecordModel.CashbookIdCreditor,
                    date = cashbookRecordModel.Date,
                    amount = cashbookRecordModel.Amount,
                    reference = cashbookRecordModel.OriginReference
                });
            }
        }

        public async Task<int> UpdateCashbookBalance(int cashbookId, decimal balance)
        {
            var sqlExpressionToUpdate = @"
                UPDATE `accountservice_db`.`cashbook`
                    SET `balance` = @balance
                    WHERE `id` = @cashbookId;";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();

                return await connection.ExecuteAsync(sqlExpressionToUpdate, new
                {
                    cashbookId,
                    balance
                });
            }
        }

        public async Task<IEnumerable<AccountModel>> GetAccounts()
        {
            var sqlExpressionToGetAccounts = @"
                SELECT `account`.`id` as Id,
                    `account`.`name` as Name,
                    `account`.`IBAN` as IBAN,
                    `account`.`email` as Email
                FROM `accountservice_db`.`account`;";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return await connection.QueryAsync<AccountModel>(sqlExpressionToGetAccounts);
            }
        }

        public async Task<IEnumerable<CashbookRecordModel>> GetMutationsByCashbookId(int cashbookId,
            DateTime beforeDate, DateTime afterDate)
        {
            var sqlExpressionToGetAccounts = @"
                SELECT `cashbookRecords`.`id` as Id,
                    `cashbookRecords`.`cashbookidFrom` as CashbookIdDebtor,
                    `cashbookRecords`.`cashbookidTo` as CashbookIdCreditor,
                    `cashbookRecords`.`date` as Date,
                    `cashbookRecords`.`amount` as Amount,
                    `cashbookRecords`.`original_reference` as OriginReference
                FROM `accountservice_db`.`cashbookRecords`
                WHERE (cashbookidFrom = @cashbookId OR cashbookidTo = @cashbookId)
                AND (date > @afterDate AND date < @beforeDate);";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return await connection.QueryAsync<CashbookRecordModel>(sqlExpressionToGetAccounts, new
                {
                    cashbookId,
                    beforeDate,
                    afterDate
                });
            }
        }

        public async Task<string> GetAccountIBANByCashbookId(int cashbookId)
        {
            var sqlExpressionToUpdate = @"
                 SELECT IBAN FROM accountservice_db.account acc
                 INNER JOIN accountservice_db.cashbook ch
                 ON acc.id = ch.accountid
                 WHERE ch.id = @cashbookId;";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();

                return await connection.QuerySingleAsync<string>(sqlExpressionToUpdate, new
                {
                    cashbookId
                });
            }
        }
    }
}
