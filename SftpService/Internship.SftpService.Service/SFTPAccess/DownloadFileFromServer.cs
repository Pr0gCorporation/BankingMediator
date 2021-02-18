using System.IO;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace Internship.SftpService.Service.SFTPAccess
{
    public sealed class DownloadFileFromServer : IFileDownloadable
    {
        private readonly SftpClient _sftpClient;

        public DownloadFileFromServer(SftpClient sftpClient)
        {
            _sftpClient = sftpClient;
        }
        
        public int Download(string pathTo, string pathFrom, bool removeFileAfterDownloading, ILogger logger)
        {
            _sftpClient.Connect();
            logger.LogInformation($"Connect to sftp: {_sftpClient.IsConnected} .\n\n");

            var files = _sftpClient.ListDirectory(pathFrom);

            var downloaded = 0;

            foreach (var file in files)
            {
                if (file.IsDirectory) continue;
                var fullPath = pathFrom + file.Name;
                logger.LogInformation($"Downloading file: {fullPath}\n\n");
                using (Stream fileStream = File.Create(pathTo + file.Name))
                {
                    DownloadFile(_sftpClient, fullPath, fileStream);
                    downloaded++;
                }

                if(!removeFileAfterDownloading) continue;
                DeleteFile(_sftpClient, fullPath);
                logger.LogInformation($"File deleted: {fullPath}\n\n");
            }
            
            _sftpClient.Disconnect();
            logger.LogInformation($"Connect from sftp: {_sftpClient.IsConnected} .\n\n");

            return downloaded;
        }

        private void DownloadFile(ISftpClient sftp, string pathFrom, Stream fileStream)
        {
            sftp.DownloadFile(pathFrom, fileStream);
        }

        private void DeleteFile(ISftpClient sftp, string pathFrom)
        {
            sftp.DeleteFile(pathFrom);
        }
    }
}