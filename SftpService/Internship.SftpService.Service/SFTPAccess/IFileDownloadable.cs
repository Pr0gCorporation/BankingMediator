using Microsoft.Extensions.Logging;

namespace Internship.SftpService.Service.SFTPAccess
{
    public interface IFileDownloadable
    {
        int Download(string pathTo, string pathFrom, bool removeFileAfterDownloading, ILogger logger);
    }
}