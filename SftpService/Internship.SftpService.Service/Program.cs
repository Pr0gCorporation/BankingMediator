using System;
using System.Linq;
using Internship.SftpService.Service.Extentions;
using Internship.SftpService.Service.Jobs;
using Internship.SftpService.Service.Jobs.Configuration;
using Internship.SftpService.Service.SFTPClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

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
                    var configuration = hostContext.Configuration;

                    services.AddHostedService<Worker>();
                    services.AddTransient<ISftpClientIntern, SftpClientIntern>();
                    services.AddSftpDownloader();
                    services.AddSftpUploader();
                    
                    var downloadJobConfiguration = new JobConfiguration(
                        configuration.GetValue<string>("JobConfig:DownloadJob:JobKey"),
                        configuration.GetValue<string>("JobConfig:DownloadJob:WithIdentity"),
                        DateTimeOffset.Now,
                        configuration.GetValue<string>("JobConfig:DownloadJob:CronSchedule")
                    );
                    var uploadJobConfiguration = new JobConfiguration(
                        configuration.GetValue<string>("JobConfig:UploadJob:JobKey"),
                        configuration.GetValue<string>("JobConfig:UploadJob:WithIdentity"),
                        DateTimeOffset.Now,
                        configuration.GetValue<string>("JobConfig:UploadJob:CronSchedule")
                    );
                    
                    services.AddQuartz(q =>  
                    {
                        // Use a Scoped container to create jobs.
                        q.UseMicrosoftDependencyInjectionScopedJobFactory();

                        // Create a "key"s for the jobs
                        var downloadJobKey = new JobKey(downloadJobConfiguration.JobKey);
                        var uploadJobKey = new JobKey(uploadJobConfiguration.JobKey);

                        // Register the jobs with the DI container
                        q.AddJob<DownloadFilesJob>(opts => opts.WithIdentity(downloadJobKey));
                        q.AddJob<UploadFilesJob>(opts => opts.WithIdentity(uploadJobKey));

                        q.AddTrigger(opts => opts
                            .ForJob(downloadJobKey)
                            .WithIdentity(downloadJobConfiguration.WithIdentity)
                            .StartAt(downloadJobConfiguration.StartAt)
                            .WithCronSchedule(downloadJobConfiguration.CronSchedule));
                        q.AddTrigger(opts => opts
                            .ForJob(uploadJobKey)
                            .WithIdentity(uploadJobConfiguration.WithIdentity)
                            .StartAt(uploadJobConfiguration.StartAt)
                            .WithCronSchedule(uploadJobConfiguration.CronSchedule));
                    });

                    // Add the Quartz.NET hosted service
                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);
                });
    }
}
