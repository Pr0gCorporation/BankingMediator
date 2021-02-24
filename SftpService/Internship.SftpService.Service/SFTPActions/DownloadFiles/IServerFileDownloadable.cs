namespace Internship.SftpService.Service.SFTPActions.DownloadFiles
{
    public interface IServerFileDownloadable
    {
        int Download(string pathTo, string pathFrom, bool removeFileAfterDownloading);
    }
}