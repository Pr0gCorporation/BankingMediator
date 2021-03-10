using System;
using System.Threading.Tasks;
using Internship.FileService.Domain.Interfaces;
using Internship.FileService.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service.Consumers
{
    public class IncomingFileConsumer : IConsumer<IncomingFile>
    {
        private readonly ILogger<IncomingFileConsumer> _logger;
        private readonly IFileRepository _repository;

        public IncomingFileConsumer(ILogger<IncomingFileConsumer> logger,
            IFileRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IncomingFile> context)
        {
            _logger.LogInformation($"Received new message: {context.MessageId}, of type {context.GetType()}");

            const bool isIncomingTransaction = true;
            try
            {
                await _repository.Add(
                    DateTime.Now, isIncomingTransaction,
                    context.Message.FileName,
                    context.Message.File);

                _logger.LogInformation($"File {context.Message.FileName} inserted successfully!");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Inserting failed {context.MessageId}");
                _logger.LogDebug($"File size (bytes): {context.Message.File.Length.ToString()}");
                throw;
            }
        }
    }
}