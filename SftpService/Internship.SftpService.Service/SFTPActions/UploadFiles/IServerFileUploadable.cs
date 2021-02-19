namespace Internship.SftpService.Service.SFTPAccess
{
    public interface IServerFileUploadable
    {
        int Upload(string pathTo, string pathFrom, bool removeFileAfterDownloading); 
    }
}