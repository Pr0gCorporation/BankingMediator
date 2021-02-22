using Internship.SftpService.Service.SFTPAccess;
using Internship.SftpService.Service.SFTPActions.UploadFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Renci.SshNet;

namespace Internship.SftpService.Service.Extentions
{
    public static class ServerFileUploadExtention
    {
        /// <summary>
        /// Add singleton class to upload files to sftp server.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSftpUploader(this IServiceCollection services)
        {
            services.AddSingleton<IServerFileUploadable, UploadFilesToServer>();
            return services;
        }
    }
}