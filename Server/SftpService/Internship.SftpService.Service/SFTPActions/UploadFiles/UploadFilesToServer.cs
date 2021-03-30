using System;
using System.IO;
using System.Text;
using Internship.SftpService.Service.SFTPClient;
using Microsoft.Extensions.Logging;

namespace Internship.SftpService.Service.SFTPActions.UploadFiles
{
    public class UploadFilesToServer : IServerFileUploadable
    {
        private readonly SftpClientIntern _sftpClient;
        private readonly ILogger<UploadFilesToServer> _logger;

        public UploadFilesToServer(SftpClientIntern sftpClient, ILogger<UploadFilesToServer> logger)
        {
            _sftpClient = sftpClient;
            _logger = logger;
        }

        public int Upload(string pathTo, string pathFrom, bool removeFileAfterDownloading = false)
        {
            _sftpClient.Connect();
            _logger.LogInformation($"Connect to sftp: {_sftpClient.IsConnected()}.\n");

            var files = Directory.EnumerateFiles(pathFrom);

            var uploaded = 0;

            foreach (var file in files)
            {
                _logger.LogWarning(file);
                using (Stream fileStream = File.OpenRead(file))
                {
                    _sftpClient.UploadFile(fileStream, pathTo + Path.GetFileName(file));
                    uploaded++;
                    _logger.LogInformation($"File uploaded: {file}\n\n");
                }

                if (!removeFileAfterDownloading) continue;
                File.Delete(file);
                _logger.LogInformation($"File deleted: {file}\n\n");
            }

            _sftpClient.Disconnect();
            _logger.LogInformation($"Connect to sftp: {_sftpClient.IsConnected()}.\n" + DateTime.Now + "\n");

            return uploaded;
        }

        public void Upload(string pathTo, byte[] file, string filename)
        {
            _sftpClient.Connect();
            _logger.LogInformation($"Connect to sftp: {_sftpClient.IsConnected()}.\n");

            using (var stream = new MemoryStream())
            {
                stream.Write(file, 0, file.Length);
                stream.Position = 0;
                _sftpClient.UploadFile(stream, pathTo + filename);
            }

            _logger.LogInformation($"File uploaded: {file}\n\n");

            _sftpClient.Disconnect();
        }
    }
}