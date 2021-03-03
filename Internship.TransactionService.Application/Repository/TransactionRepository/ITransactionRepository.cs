using Internship.TransactionService.Domain.Models;

namespace Internship.TransactionService.Application.Repository.TransactionRepository
{
    public interface ITransactionRepository : IGenericRepository<TransactionModel>
    {
    }
}