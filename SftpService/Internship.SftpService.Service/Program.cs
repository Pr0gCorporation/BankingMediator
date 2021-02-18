using Internship.SftpService.Service.SFTPAccess;
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
                    
                    services.AddSingleton<IFileDownloadable, DownloadFileFromServer>(downloader => 
                        new DownloadFileFromServer(
                            "localhost",
                            "foo", 
                            "pass"));
                });
    }
}
