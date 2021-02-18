using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Internship.SftpService.Service.SFTPAccess;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Internship.SftpService.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IFileDownloadable _downloader;

        public Worker(ILogger<Worker> logger, IFileDownloadable downloader)
        {
            _logger = logger;
            _downloader = downloader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Message from ExecuteAsync before the LongWorkload method.\n\n\n");

            _downloader.Download(@"./downloads/pogchamp.txt", "upload/out/", "pogchamp.txt", _logger);

            // LongWorkload(stoppingToken);
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
