using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionModel>> GetAll();
        Task<TransactionModel> GetById(int id);
        Task<int> Add(TransactionModel transactionModel);
        Task<int> UpdateStatus(TransactionStatusModel transactionStatusModel);
        Task<TransactionStatusModel> GetStatusByTransactionId(int transaction_id);
        Task<int> GetTransactionPrimaryKeyByTransactionId(Guid transaction_id);
    }
}