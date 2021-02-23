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
        private readonly IBus _publishEndpoint;

        public Worker(ILogger<Worker> logger, HostBuilderContext hostBuilderContext, IBus publishEndpoint)
        {
            _hostBuilderContext = hostBuilderContext;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await LongWorkload(stoppingToken);
        }

        private async Task LongWorkload(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _publishEndpoint.Publish(new Message{Text = "Hi, Ruslan, it's your poggers pc."}, cancellationToken);
                await Task.Delay(1500, cancellationToken);
            }
        }
    }
}
