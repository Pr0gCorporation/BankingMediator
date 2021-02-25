using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Internship.FileService.Service.Converters;
using Internship.FileService.Service.DBAccess;
using Internship.FileService.Service.Models;
using Internship.SftpService.Service.Models;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service.Consumers
{
    public class TransactionConsumer : IConsumer<FileModel>
    {
        private readonly ILogger<TransactionConsumer> _logger;
        private readonly IByteConvertable _converter;
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly InsertTransactionToDb _inserter;

        public TransactionConsumer(ILogger<TransactionConsumer> logger, IByteConvertable converter,
            HostBuilderContext hostBuilderContext, InsertTransactionToDb inserter)
        {
            this._logger = logger;
            _converter = converter;
            _hostBuilderContext = hostBuilderContext;
            _inserter = inserter;
        }

        public async Task Consume(ConsumeContext<FileModel> context)
        {
            _logger.LogWarning($"Look! I've got a new file: {context.Message.Name}, " +
                               $"\ndate = {context.Message.Date}, " +
                               $"\nbytes[] = {context.Message.File}\n");
            
            // TODO: add try/catch

            using var streamReader = _converter.Convert(context.Message.File);
            var xmlSerializer = new XmlSerializer(typeof(TransactionXmlModel));
            var transaction = (TransactionXmlModel) xmlSerializer.Deserialize(streamReader);

            // Insert to DB

            var configuration = _hostBuilderContext.Configuration;
            
            if (transaction != null)
            {
                transaction.FileName = context.Message.Name;
                await _inserter.Insert(
                    new SqlConnection(configuration.GetConnectionString("MSSQLSERVERConnection")),
                    transaction);
            }
            else
            {
                _logger.LogWarning($"Wrong transaction!");
            }
        }
    }
}