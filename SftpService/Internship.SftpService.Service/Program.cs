using Internship.SftpService.Service.Jobs;
using Internship.SftpService.Service.SFTPAccess;
using Internship.SftpService.Service.SFTPClientFactory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

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
                    services.AddSingleton<IFileDownloadable, DownloadFileFromServer>(downloader => 
                        new DownloadFileFromServer(
                            sftpClientFactory.GetSftpClient()));

                    services.AddTransient<JobFactory>();
                    services.AddScoped<DownloadFilesJob>();
                });
    }
}
