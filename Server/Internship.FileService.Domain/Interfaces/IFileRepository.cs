using System;
using System.Threading.Tasks;

namespace Internship.FileService.Domain.Interfaces
{
    public interface IFileRepository
    {
        Task<int> Add(DateTime date, bool type, string filename, byte[] file);
    }
}