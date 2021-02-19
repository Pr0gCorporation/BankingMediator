using Microsoft.Extensions.DependencyInjection;
using Internship.SftpService.Service.SFTPAccess;
using Internship.SftpService.Service.SFTPActions.DownloadFiles;
using Internship.SftpService.Service.SFTPClient;
using Microsoft.Extensions.Logging;

namespace Internship.SftpService.Service.Extentions
{
    public static class ServerFileDownloadExtention
    {
        public static IServiceCollection AddSftpDownloader(this IServiceCollection services, ISFTPClientFactory sftpClientFactory)
        {
            services.AddSingleton<IServerFileDownloadable, DownloadFilesFromServer>(provider => 
                new DownloadFilesFromServer(
                    sftpClientFactory.GetSftpClient(), 
                    provider.GetService<ILogger<DownloadFilesFromServer>>()));
            return services;
        }
    }
}