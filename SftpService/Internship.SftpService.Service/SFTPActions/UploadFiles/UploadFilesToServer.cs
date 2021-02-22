using System;
using System.IO;
using Internship.SftpService.Service.SFTPAccess;
using Internship.SftpService.Service.SFTPClient;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace Internship.SftpService.Service.SFTPActions.UploadFiles
{
    public class UploadFilesToServer : IServerFileUploadable
    {
        private readonly ISftpClientIntern _sftpClient;
        private readonly ILogger<UploadFilesToServer> _logger;

        public UploadFilesToServer(ISftpClientIntern sftpClient, ILogger<UploadFilesToServer> logger)
        {
            _sftpClient = sftpClient;
            _logger = logger;
        }
        
        public int Upload(string pathTo, string pathFrom, bool removeFileAfterDownloading = false)
        {
            _sftpClient.Connect();
            _logger.LogInformation($"Connect to sftp: {_sftpClient.IsConnected()}.-------------------\n");

            var files = Directory.EnumerateFiles(pathFrom);
            
            var uploaded = 0;

            foreach (var file in files)
            {
                _logger.LogWarning(file);
                using (Stream fileStream = File.OpenRead(file))
                {
                    _sftpClient.UploadFile(fileStream, pathTo + Path.GetFileName(file));
                    uploaded++;
                }
                
                if(!removeFileAfterDownloading) continue;
                File.Delete(file);
                _logger.LogInformation($"File deleted: {file}\n\n");
            }

            _sftpClient.Disconnect();
            _logger.LogInformation($"Connect to sftp: {_sftpClient.IsConnected()}.\n" + DateTime.Now+ "\n");
            
            return uploaded;
        }
    }
}