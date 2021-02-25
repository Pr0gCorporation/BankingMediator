using System.Data.SqlTypes;

namespace Internship.SftpService.Service.Models
{
    public class FileModel
    {
        public byte[] File { get; set; }
        public string Name { get; set; }
        public SqlDateTime Date { get; set; }
    }
}