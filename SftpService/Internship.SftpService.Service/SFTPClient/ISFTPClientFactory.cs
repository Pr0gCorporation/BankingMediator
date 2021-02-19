using Renci.SshNet;

namespace Internship.SftpService.Service.SFTPClient
{
    public interface ISFTPClientFactory
    {
        SftpClient GetSftpClient();
    }
}