using System.Collections.Generic;
using Internship.SftpService.Service.DTOs;

namespace Internship.SftpService.Service.Messages
{
    public class TransactionsData
    {
        public IEnumerable<FileDto> Transactions { get; set; }
    }
}