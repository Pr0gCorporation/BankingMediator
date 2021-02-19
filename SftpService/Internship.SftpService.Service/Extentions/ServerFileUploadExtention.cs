using Internship.SftpService.Service.SFTPAccess;
using Internship.SftpService.Service.SFTPActions.UploadFiles;
using Internship.SftpService.Service.SFTPClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Internship.SftpService.Service.Extentions
{
    public static class ServerFileUploadExtention
    {
        public static IServiceCollection AddSftpUploader(this IServiceCollection services, ISFTPClientFactory sftpClientFactory)
        {
            services.AddSingleton<IServerFileUploadable, UploadFilesToServer>(provider => 
                new UploadFilesToServer(
                    sftpClientFactory.GetSftpClient(), 
                    provider.GetService<ILogger<UploadFilesToServer>>()));
            return services;
        }
    }
}