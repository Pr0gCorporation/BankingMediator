using Internship.FileService.Service.Consumers;
using Internship.FileService.Service.DBAccess;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Internship.FileService.Service
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
                    var configuration = hostContext.Configuration;
                    
                    services.AddScoped<InsertTransactionToDb>();
                    
                    services.AddMassTransit(config => {
                        config.AddConsumer<IncomingFileConsumer>();
                        config.AddConsumer<TransactionToFileConsumer>();

                        config.UsingRabbitMq((ctx, cfg) => {
                            cfg.Host(configuration.GetValue<string>("BusConfig:Host"));

                            cfg.ReceiveEndpoint("file_receive", c => {
                                c.ConfigureConsumer<IncomingFileConsumer>(ctx);
                            });
                            
                            cfg.ReceiveEndpoint("file_send", c => {
                                c.ConfigureConsumer<TransactionToFileConsumer>(ctx);
                            });
                        });
                    });

                    services.AddMassTransitHostedService();
                });
    }
}