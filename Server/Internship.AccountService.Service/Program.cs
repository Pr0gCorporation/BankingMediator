using Internship.AccountService.Domain.Interfaces;
using Internship.AccountService.Service.Consumers;
using Internship.AccountService.Infrastructure.DAL;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

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
                });
    }
}
