using System;
using System.IO;
using System.Transactions;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace Internship.SftpService.Service.SFTPAccess
{
    public class UploadFilesToServer : IServerFileUploadable
    {
        private readonly SftpClient _sftpClient;
        private readonly ILogger<UploadFilesToServer> _logger;

        public UploadFilesToServer(SftpClient sftpClient, ILogger<UploadFilesToServer> logger)
        {
            _sftpClient = sftpClient;
            _logger = logger;
        }
        
        public int Upload(string pathTo, string pathFrom, bool removeFileAfterDownloading = false)
        {
            _sftpClient.Connect();
            _logger.LogInformation($"Connect to sftp: {_sftpClient.IsConnected} .\n\n");

            var files = Directory.EnumerateFiles(pathFrom);

            foreach (var file in files)
            {
                _logger.LogDebug(file);
            }

            var uploaded = 0;
/*
 
            foreach (var file in files)
            {
                if (file.IsDirectory) continue;
                var fullPath = pathFrom + file.Name;
                _logger.LogInformation($"Downloading file: {fullPath}\n\n");
                using (Stream fileStream = File.Create(pathTo + file.Name))
                {
                    _sftpClient.DownloadFile(fullPath, fileStream);
                    uploaded++;
                }

                if(!removeFileAfterDownloading) continue;
                _sftpClient.DeleteFile(fullPath);
                _logger.LogInformation($"File deleted: {fullPath}\n\n");
            }
            
*/

            _sftpClient.Disconnect();
            _logger.LogInformation($"Connect to sftp: {_sftpClient.IsConnected} .\n\n");
            
            return uploaded;
        }
    }
}