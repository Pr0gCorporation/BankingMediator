using Internship.AccountService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internship.AccountService.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<int> InsertCashbookRecord(CashbookRecordModel cashbookRecordModel);
        Task<int> GetCashbookPrimaryKeyByAccountId(int accountId);
        Task<int> GetAccountPrimaryKeyByIBAN(string IBAN);
        Task<decimal> GetSumOfCashbookRecordsByCashbookId(int cashbookId);
        Task<int> UpdateCashbookBalance(int cashbookId, decimal balance);
    }
}
