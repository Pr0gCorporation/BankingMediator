using System;
using Internship.SftpService.Service.Consumers;
using Internship.SftpService.Service.Extentions;
using Internship.SftpService.Service.FileActions.FileReader;
using Internship.SftpService.Service.Jobs;
using Internship.SftpService.Service.Jobs.Configuration;
using Internship.SftpService.Service.Publishers.FilePublisher;
using Internship.SftpService.Service.SFTPClient;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;

namespace Internship.SftpService.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Application Starting Up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
                throw;
            }
            finally
            {
                Log.Information("Application Stopping Down");
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    services.AddHostedService<Worker>();
                    services.AddTransient<SftpClientIntern>();
                    services.AddSftpDownloader();
                    services.AddSftpUploader();
                    services.AddScoped<TransactionFilePublisher>();
                    services.AddScoped<ReadXmlFiles>();
                    
                    services.AddMassTransit(config =>
                    {
                        config.AddConsumer<OutgoingFileConsumer>();
                        
                        config.UsingRabbitMq((ctx, cfg) =>
                        {
                            cfg.Host(configuration.GetValue<string>("BusConfig:Host"));
                            
                             cfg.ReceiveEndpoint("file_save", c => {
                                 c.ConfigureConsumer<OutgoingFileConsumer>(ctx);
                             });
                        });
                    });
                    
                    services.AddMassTransitHostedService();

                    services.AddQuartz(q =>
                    {
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

                        // Use a Scoped container to create jobs.
                        q.UseMicrosoftDependencyInjectionScopedJobFactory();

                        // Create a "key"s for the jobs
                        // var downloadJobKey = new JobKey(downloadJobConfiguration.JobKey);
                        // var uploadJobKey = new JobKey(uploadJobConfiguration.JobKey);

                        // Register the jobs with the DI container
                        // q.AddJob<DownloadPublishFilesJob>(opts => opts.WithIdentity(downloadJobKey));
                        // q.AddJob<UploadFilesJob>(opts => opts.WithIdentity(uploadJobKey));

                        // q.AddTrigger(opts => opts
                        //     .ForJob(downloadJobKey)
                        //     .WithIdentity(downloadJobConfiguration.WithIdentity)
                        //     .StartAt(downloadJobConfiguration.StartAt)
                        //     .WithCronSchedule(downloadJobConfiguration.CronSchedule));
                        // q.AddTrigger(opts => opts
                        //     .ForJob(uploadJobKey)
                        //     .WithIdentity(uploadJobConfiguration.WithIdentity)
                        //     .StartAt(uploadJobConfiguration.StartAt)
                        //     .WithCronSchedule(uploadJobConfiguration.CronSchedule));
                    });

                    // Add the Quartz.NET hosted service
                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);
                });
    }
}