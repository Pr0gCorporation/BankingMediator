using System.Collections.Generic;

namespace Internship.SftpService.Service.SFTPActions.DownloadFiles
{
    public interface IServerFileDownloadable
    {
        List<byte[]> Download(string pathFrom, bool removeFileAfterDownloading);
    }
}