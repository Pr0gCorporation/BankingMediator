using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internship.TransactionService.Application.Repository
{
    public interface IGenericRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
    }
}