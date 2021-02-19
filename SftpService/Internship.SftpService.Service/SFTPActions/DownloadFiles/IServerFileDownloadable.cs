using Microsoft.Extensions.Logging;

namespace Internship.SftpService.Service.SFTPAccess
{
    public interface IServerFileDownloadable
    {
        int Download(string pathTo, string pathFrom, bool removeFileAfterDownloading);
    }
}