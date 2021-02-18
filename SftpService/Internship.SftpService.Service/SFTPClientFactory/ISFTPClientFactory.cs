using Renci.SshNet;

namespace Internship.SftpService.Service.SFTPClientFactory
{
    public interface ISFTPClientFactory
    {
        SftpClient GetSftpClient();
    }
}