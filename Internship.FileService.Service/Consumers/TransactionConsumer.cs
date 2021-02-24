using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Internship.FileService.Service.Converters;
using Internship.FileService.Service.Models;
using Internship.SftpService.Service.DTOs;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service.Consumers
{
    public class TransactionConsumer : IConsumer<FileDto>
    {
        private readonly ILogger<TransactionConsumer> _logger;
        private readonly IByteConvertable _converter;

        public TransactionConsumer(ILogger<TransactionConsumer> logger, IByteConvertable converter)
        {
            this._logger = logger;
            _converter = converter;
        }

        public async Task Consume(ConsumeContext<FileDto> context)
        {
            _logger.LogWarning($"Look! I've got a new file: {context.Message.Name}, " +
                               $"\ndate = {context.Message.Date}, " +
                               $"\nbytes[] = {context.Message.File}\n");

            using var streamReader = await _converter.Convert(context.Message.File);
            var xmlSerializer = new XmlSerializer(typeof(TransactionModel));
            var transaction = (TransactionModel) xmlSerializer.Deserialize(streamReader);

            if (transaction != null)
            {
                transaction.FileName = context.Message.Name;
                Console.WriteLine(transaction.Id.ToString());
                Console.WriteLine(transaction.FileName);
                Console.WriteLine(transaction.Date);
                Console.WriteLine(transaction.From);
                Console.WriteLine(transaction.To);
            }
        }
    }
}