using Dapper;
using Internship.AccountService.Domain.Interfaces;
using Internship.AccountService.Domain.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
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
            return ((await this.GetSentMoney(cashbookId)) * -1) + await this.GetReceivedMoney(cashbookId);
        }
        
        private async Task<decimal> GetSentMoney(int cashbookIdFrom)
        {
            var sqlExpressionToGetSentMoney = @"
                SELECT SUM(amount)
                FROM accountservice_db.cashbookRecords
                WHERE cashbookidFrom = @cashbookIdFrom;";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return await connection.QuerySingleAsync<decimal>(
                    sqlExpressionToGetSentMoney, new
                    {
                        cashbookIdFrom
                    });
            }
        }

        private async Task<decimal> GetReceivedMoney(int cashbookIdTo)
        {
            var sqlExpressionToGetSentMoney = @"
                SELECT SUM(amount)
                FROM accountservice_db.cashbookRecords
                WHERE cashbookidTo = @cashbookIdTo;";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return await connection.QuerySingleAsync<decimal>(
                    sqlExpressionToGetSentMoney, new
                    {
                        cashbookIdTo
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
                    cashbookidFrom = cashbookRecordModel.CashbookIdFrom,
                    cashbookidTo = cashbookRecordModel.CashbookIdTo,
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
    }
}
