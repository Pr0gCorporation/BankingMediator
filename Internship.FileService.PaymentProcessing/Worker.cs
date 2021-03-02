using System;
using System.Threading;
using System.Threading.Tasks;
using Internship.FileService.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.PaymentProcessing
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBus _publishEndpoint;

        public Worker(ILogger<Worker> logger, IBus publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var payment = new Transaction()
            {
                TransactionId = DateTime.Now.Second + DateTime.Now.Millisecond,
                Sum = (decimal) 2020.52 + DateTime.Now.Second,
                Date = DateTime.Now,
                Creditor = "Will Smith",
                Debtor = "Jordan Smith"
            };
            
            await _publishEndpoint.Publish(payment, stoppingToken);
        }
    }
}