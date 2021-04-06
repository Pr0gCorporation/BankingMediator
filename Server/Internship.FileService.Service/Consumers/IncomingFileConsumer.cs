using System;
using System.IO;
using System.Threading.Tasks;
using Internship.FileService.Domain.Interfaces;
using Internship.FileService.Service.Publishers;
using Internship.Shared.DTOs.File;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service.Consumers
{
    public class IncomingFileConsumer : IConsumer<IncomingFileDto>
    {
        private readonly ILogger<IncomingFileConsumer> _logger;
        private readonly IFileRepository _repository;
        private IncomingTransactionFilePublisher _publisher;
        private readonly IBus _bus;

        public IncomingFileConsumer(ILogger<IncomingFileConsumer> logger,
            IFileRepository repository, IBus bus)
        {
            _logger = logger;
            _repository = repository;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<IncomingFileDto> context)
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

                _publisher = (Path.GetExtension(context.Message.FileName)) switch
                {
                    ".xml" => new IncomingTransactionXmlFilePublisher(_bus),
                    ".json" => new IncomingTransactionJsonFilePublisher(_bus),
                    ".csv" => new IncomingTransactionCsvFilePublisher(_bus),
                    _ => null,
                };

                if (_publisher is not null)
                {
                    _publisher.PublishIncomingTransaction(context.Message.File);
                    _logger.LogInformation($"File {context.Message.FileName} published successfully!");
                }
                else
                {
                    _logger.LogInformation($"File {context.Message.FileName} publish failed! Probably wrong file extention.");
                }
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