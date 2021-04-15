using Internship.AccountService.Domain.Interfaces;
using Internship.AccountService.Service.Consumers;
using Internship.AccountService.Infrastructure.DAL;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using Quartz;
using Internship.AccountService.Service.Jobs;

namespace Internship.AccountService.Service
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
                    services.AddHostedService<Worker>();
                    var configuration = hostContext.Configuration;

                    services.AddTransient<IAccountRepository, AccountRepository>();

                    services.AddMassTransit(config =>
                    {
                        config.AddConsumer<UpdateAccountBalanceConsumer>();

                        config.UsingRabbitMq((ctx, cfg) =>
                        {
                            cfg.Host(configuration.GetValue<string>("BusConfig:Host"));
                             
                            cfg.ReceiveEndpoint("update_balance", c =>
                            {
                                c.ConfigureConsumer<UpdateAccountBalanceConsumer>(ctx);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();

                    services.AddQuartz(q =>
                    {
                        var endOfDayReportJobConfiguration = new JobConfiguration(
                            configuration.GetValue<string>("JobConfig:GenerateReportJob:JobKey"),
                            configuration.GetValue<string>("JobConfig:GenerateReportJob:WithIdentity"),
                            DateTimeOffset.Now,
                            configuration.GetValue<string>("JobConfig:GenerateReportJob:TestCron")
                        );

                        // Use a Scoped container to create jobs.
                        q.UseMicrosoftDependencyInjectionScopedJobFactory();

                        // Create a "key"s for the jobs
                        var endOfDayReportJobKey = new JobKey(endOfDayReportJobConfiguration.JobKey);

                        // Register the jobs with the DI container
                        q.AddJob<EndOfDayReportingJob>(opts => opts.WithIdentity(endOfDayReportJobKey));

                        q.AddTrigger(opts => opts
                             .ForJob(endOfDayReportJobKey)
                             .WithIdentity(endOfDayReportJobConfiguration.WithIdentity)
                             .StartAt(endOfDayReportJobConfiguration.StartAt)
                             .WithCronSchedule(endOfDayReportJobConfiguration.CronSchedule));
                    });

                    // Add the Quartz.NET hosted service
                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);
                });
    }
}
