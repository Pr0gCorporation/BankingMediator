using System;
using Internship.SftpService.Service.Extentions;
using Internship.SftpService.Service.Jobs;
using Internship.SftpService.Service.Jobs.Configuration;
using Internship.SftpService.Service.SFTPClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Internship.SftpService.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    ISFTPClientFactory sftpClientFactory = new SftpClientFactory();
                    services.AddDownloader(sftpClientFactory);
                    services.AddUploader(sftpClientFactory);

                    var downloadConfiguration = new JobConfiguration("DownloadFilesJob", 
                        "DownloadFilesJob-trigger", 
                        DateTimeOffset.Now, 
                        "0/15 * * * * ?");
                    
                    var uploadConfiguration = new JobConfiguration("UploadFilesJob", 
                        "UploadFilesJob-trigger", 
                        DateTimeOffset.Now, 
                        "0/15 * * * * ?");

                    //services.AddJob<DownloadFilesJob>(downloadConfiguration);
                    services.AddJob<UploadFilesJob>(uploadConfiguration);
                });
    }
}
