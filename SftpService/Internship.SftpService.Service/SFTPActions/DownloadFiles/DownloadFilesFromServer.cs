using System.IO;
using Internship.SftpService.Service.SFTPClient;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace Internship.SftpService.Service.SFTPActions.DownloadFiles
{
    public sealed class DownloadFilesFromServer : IServerFileDownloadable
    {
        private readonly SftpClientIntern _sftpClient;
        private readonly ILogger<DownloadFilesFromServer> _logger;

        public DownloadFilesFromServer(SftpClientIntern sftpClient, ILogger<DownloadFilesFromServer> logger)
        {
            _sftpClient = sftpClient;
            _logger = logger;
        }
        
        public int Download(string pathTo, string pathFrom, bool removeFileAfterDownloading = false)
        {
            _sftpClient.Connect();
            _logger.LogInformation($"Connect to sftp: {_sftpClient.IsConnected()} .\n\n");

            var files = _sftpClient.ListDirectory(pathFrom);

            var downloaded = 0;

            foreach (var file in files)
            {
                if (file.IsDirectory) continue;
                var fullPath = pathFrom + file.Name;
                _logger.LogInformation($"Downloading file: {fullPath}\n\n");
                using (Stream fileStream = File.Create(pathTo + file.Name))
                {
                    _sftpClient.DownloadFile(fullPath, fileStream);
                    downloaded++;
                }

                if(!removeFileAfterDownloading) continue;
                _sftpClient.DeleteFile(fullPath);
                _logger.LogInformation($"File deleted: {fullPath}\n\n");
            }
            
            _sftpClient.Disconnect();
            _logger.LogInformation($"Connect to sftp: {_sftpClient.IsConnected()} .\n\n");

            return downloaded;
        }
    }
}