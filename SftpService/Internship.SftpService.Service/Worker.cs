using System;
using System.Threading;
using System.Threading.Tasks;
using Internship.SftpService.Service.Jobs;
using Internship.SftpService.Service.SFTPAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Spi;

namespace Internship.SftpService.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider  serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
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
