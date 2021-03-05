using System.Threading;
using System.Threading.Tasks;
using MassTransit;
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
            //await LongWorkload(stoppingToken);
        }

        private async Task LongWorkload(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}