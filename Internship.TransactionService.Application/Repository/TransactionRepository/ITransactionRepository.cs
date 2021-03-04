using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.Application.Repository.TransactionRepository
{
    public interface ITransactionRepository : IGenericRepository<TransactionModel>
    {
        Task<TransactionModel> GetByTransactionId(Guid transactionId);
    }
}