using Internship.FileService.Domain.Interfaces;
using Internship.FileService.Infrastructure.DAL;
using Internship.FileService.Service.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Internship.FileService.Service
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

                    services.AddTransient<IFileRepository, FileRepository>();

                    services.AddMassTransit(config =>
                    {
                        config.AddConsumer<IncomingFileConsumer>();
                        config.AddConsumer<TransactionToFileConsumer>();
                        config.AddConsumer<EndOfDayReportedConsumer>();

                        config.UsingRabbitMq((ctx, cfg) =>
                        {
                            cfg.Host(configuration.GetValue<string>("BusConfig:Host"));

                            cfg.ReceiveEndpoint("file_receive", c =>
                            {
                                c.ConfigureConsumer<IncomingFileConsumer>(ctx);
                            });

                            cfg.ReceiveEndpoint("file_send", c =>
                            {
                                c.ConfigureConsumer<TransactionToFileConsumer>(ctx);
                            });

                            cfg.ReceiveEndpoint("report_generated", c =>
                            {
                                c.ConfigureConsumer<EndOfDayReportedConsumer>(ctx);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();
                });
    }
}