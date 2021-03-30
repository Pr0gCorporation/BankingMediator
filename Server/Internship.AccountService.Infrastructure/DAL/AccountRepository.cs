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
            var sqlExpressionToGetSumOfCashbookRecordsByCashbookId = @"
                SELECT COALESCE(SUM(amount), 0) AS currentBalance FROM accountservice_db.cashbookRecords
                WHERE cashbookid = 1;";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<decimal>(
                    sqlExpressionToGetSumOfCashbookRecordsByCashbookId, new
                    {
                        cashbookId
                    });
            }
        }

        public async Task<int> InsertCashbookRecord(CashbookRecordModel cashbookRecordModel)
        {
            var sqlExpressionToInsert = @"
                INSERT INTO `accountservice_db`.`cashbookRecords`
                    (`cashbookid`, `date`, `amount`, `original_reference`)
                    VALUES
                    (@cashbookId, @date, @amount, @reference);";

            using (var connection = new MySqlConnection(
                _configuration.GetConnectionString(ConnectionStringName)))
            {
                connection.Open();

                return await connection.ExecuteAsync(sqlExpressionToInsert, new
                {
                    cashbookId = cashbookRecordModel.CashbookId,
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
