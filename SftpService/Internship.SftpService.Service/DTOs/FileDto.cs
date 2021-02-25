using System;
using System.Data.SqlTypes;

namespace Internship.SftpService.Service.DTOs
{
    public class FileDto
    {
        public byte[] File { get; set; }
        public string Name { get; set; }
        public SqlDateTime Date { get; set; }
    }
}