using Microsoft.Extensions.DependencyInjection;
using Internship.SftpService.Service.SFTPActions.DownloadFiles;
using Internship.SftpService.Service.SFTPClient;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace Internship.SftpService.Service.Extentions
{
    public static class ServerFileDownloadExtention
    {
        /// <summary>
        /// Add singleton class to download files from sftp server.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSftpDownloader(this IServiceCollection services)
        {
            services.AddSingleton<IServerFileDownloadable, DownloadFilesFromServer>();
            return services;
        }
    }
}