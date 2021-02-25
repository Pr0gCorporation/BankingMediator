namespace Internship.SftpService.Service.SFTPActions.UploadFiles
{
    public interface IServerFileUploadable
    {
        int Upload(string pathTo, string pathFrom, bool removeFileAfterDownloading); 
    }
}