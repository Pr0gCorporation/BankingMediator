using Renci.SshNet;

namespace Internship.SftpService.Service.SFTPClientFactory
{
    public class SftpClientFactory : ISFTPClientFactory
    {
        public SftpClient GetSftpClient()
        {
            var connectionInfo = new ConnectionInfo("localhost", 2222, "foo", new PasswordAuthenticationMethod("foo", "pass"));

            return new SftpClient(connectionInfo);
        }
    }
}