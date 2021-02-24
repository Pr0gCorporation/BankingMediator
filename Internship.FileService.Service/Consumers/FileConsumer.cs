using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Internship.FileService.Service.DTOs;
using Internship.SftpService.Service.DTOs;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service.Consumers
{
    public class FileConsumer : IConsumer<FileDto>
    {
        private readonly ILogger<FileConsumer> _logger;

        public FileConsumer(ILogger<FileConsumer> logger)
        {
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<FileDto> context)
        {
            _logger.LogWarning($"Look! I've got a new file: {context.Message.Name}, " +
                               $"\ndate = {context.Message.Date}, " +
                               $"\nbytes[] = {context.Message.File}\n");

            await using var memoryStream = new MemoryStream(context.Message.File);
            using var streamReader = new StreamReader(memoryStream);
            var xmlSerializer = new XmlSerializer(typeof(TransactionDto));
            var transaction = (TransactionDto)xmlSerializer.Deserialize(streamReader);
            if (transaction != null)
            {
                Console.WriteLine(transaction.Id.ToString());
                Console.WriteLine(transaction.FileName);
                Console.WriteLine(transaction.Date);
                Console.WriteLine(transaction.From);
                Console.WriteLine(transaction.To);
            }
        }
    }
}