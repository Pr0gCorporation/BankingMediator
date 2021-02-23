using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Renci.SshNet.Messages;

namespace Internship.SftpService.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HostBuilderContext _hostBuilderContext;

        public Worker(ILogger<Worker> logger, HostBuilderContext hostBuilderContext)
        {
            _hostBuilderContext = hostBuilderContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var configuration = _hostBuilderContext.Configuration;
                sbc.Host(configuration.GetValue<string>("BusConfig:Host"));

                sbc.ReceiveEndpoint("test_queue", ep =>
                {
                    ep.Handler<Message>(context => Console.Out.WriteLineAsync($"Received: {context.Message.Text}"));
                });
            });

            await bus.StartAsync(stoppingToken);

            await bus.Publish(new Message{Text = "Hi, Ruslan, it's your poggers pc."}, stoppingToken);
            
            Console.WriteLine("Press any key to exit");
            await Task.Run(Console.ReadKey, stoppingToken);
        
            await bus.StopAsync(stoppingToken);
        }

        private async Task LongWorkload(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Workload running at: {time}", DateTimeOffset.Now);
                await Task.Delay(500, cancellationToken);
            }
        }
    }
}
