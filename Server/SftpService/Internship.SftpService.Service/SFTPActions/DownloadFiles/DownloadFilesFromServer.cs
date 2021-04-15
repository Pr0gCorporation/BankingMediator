using System.Collections.Generic;
using System.IO;
using Internship.SftpService.Service.SFTPClient;
using Microsoft.Extensions.Logging;

namespace Internship.SftpService.Service.SFTPActions.DownloadFiles
{
    public sealed class DownloadFilesFromServer : IServerFileDownloadable
    {
        private readonly SftpBankClient _sftpClient;
        private readonly ILogger<DownloadFilesFromServer> _logger;

        public DownloadFilesFromServer(SftpBankClient sftpClient, ILogger<DownloadFilesFromServer> logger)
        {
            _sftpClient = sftpClient;
            _logger = logger;
        }

        public List<byte[]> Download(string pathFrom, bool removeFileAfterDownloading = false)
        {
            _sftpClient.Connect();
            var files = _sftpClient.ListDirectory(pathFrom);
            List<byte[]> byteArrayFiles = new List<byte[]>();
            foreach (var file in files)
            {
                if (file.IsDirectory) continue;
                var fullPath = pathFrom + file.Name;
                using var fileStream = new MemoryStream();
                _sftpClient.DownloadFile(fullPath, fileStream);
                byteArrayFiles.Add(fileStream.ToArray());

                if (!removeFileAfterDownloading) continue;
                _sftpClient.DeleteFile(fullPath);
            }
            _sftpClient.Disconnect();
            return byteArrayFiles;
        }
    }
}