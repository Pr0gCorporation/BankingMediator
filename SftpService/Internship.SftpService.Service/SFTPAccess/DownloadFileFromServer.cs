using System.IO;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace Internship.SftpService.Service.SFTPAccess
{
    public class DownloadFileFromServer : IDownloadable
    {
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;

        public DownloadFileFromServer(string host, string username, string password)
        {
            _host = host;
            _username = username;
            _password = password;
        }
        
        public int Download(string fileName, string path, string file, ILogger logger)
        {    
            var connectionInfo = new ConnectionInfo(_host, 2222, _username, new PasswordAuthenticationMethod(_username, _password));
            logger.LogInformation($"Initialize connection info instance. \nHost: {connectionInfo.Host}," +
                                   $"Port: {connectionInfo.Port}," +
                                   $"Password: {connectionInfo.ProxyPassword}.\n\n");

            var fullpaths = path + file;

            using var sftp = new SftpClient(connectionInfo);
            logger.LogInformation($"Initialize sftp client instance: {sftp.BufferSize} .\n\n");

            sftp.Connect();
            logger.LogInformation($"Connect to sftp: {sftp.IsConnected} .\n\n");
            using (Stream fileStream = File.Create(fileName))
            {
                sftp.DownloadFile(fullpaths, fileStream);
                logger.LogInformation($"Download file: \n{sftp.ReadAllText(fullpaths)} .\n\n");

                logger.LogInformation($"Before file deleted: \n{sftp.ListDirectory(path)} .\n\n");
                sftp.DeleteFile(fullpaths);
                logger.LogInformation($"After file deleted: \n{sftp.ListDirectory(path)} .\n\n");
            }
            sftp.Disconnect();
            logger.LogInformation($"Connect from sftp: {sftp.IsConnected} .\n\n");

            return 0;
        }
    }
}