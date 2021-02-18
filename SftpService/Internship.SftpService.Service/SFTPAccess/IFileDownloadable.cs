using Microsoft.Extensions.Logging;

namespace Internship.SftpService.Service.SFTPAccess
{
    public interface IFileDownloadable
    {
        int Download(string name, string path, string file, ILogger logger);
    }
}