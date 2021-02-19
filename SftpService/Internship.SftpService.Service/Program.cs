using Internship.SftpService.Service.Jobs;
using Internship.SftpService.Service.SFTPAccess;
using Internship.SftpService.Service.SFTPClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Renci.SshNet;

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
                    services.AddSingleton<IServerFileDownloadable, DownloadFilesFromServer>(provider => 
                        new DownloadFilesFromServer(
                            sftpClientFactory.GetSftpClient(), 
                            provider.GetService<ILogger<DownloadFilesFromServer>>()));
                    services.AddSingleton<IServerFileUploadable, UploadFilesToServer>(provider => 
                        new UploadFilesToServer(
                            sftpClientFactory.GetSftpClient(), 
                            provider.GetService<ILogger<UploadFilesToServer>>()));
                    
                    // Add the required Quartz.NET services
                    services.AddQuartz(q =>  
                    {
                        // Use a Scoped container to create jobs.
                        q.UseMicrosoftDependencyInjectionScopedJobFactory();

                        // Create a "key" for the job
                        var jobKey = new JobKey("DownloadFilesJob");

                        // Register the job with the DI container
                        q.AddJob<UploadFilesJob>(opts => opts.WithIdentity(jobKey));

                        // Create a trigger for the job
                        q.AddTrigger(opts => opts
                            .ForJob(jobKey) // link to the DownloadFilesJob
                            .StartNow()
                            .WithIdentity("DownloadFilesJob-trigger") // give the trigger a unique name
                            .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds
                    });

                    // Add the Quartz.NET hosted service
                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);
                });
    }
}
