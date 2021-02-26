using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Internship.FileService.Domain.Models;
using Internship.FileService.Service.DBAccess;
using Internship.SftpService.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service.Consumers
{
    public class TransactionConsumer : IConsumer<FileModel>
    {
        private readonly ILogger<TransactionConsumer> _logger;
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly InsertTransactionToDb _inserter;

        public TransactionConsumer(ILogger<TransactionConsumer> logger,
            HostBuilderContext hostBuilderContext, InsertTransactionToDb inserter)
        {
            _logger = logger;
            _hostBuilderContext = hostBuilderContext;
            _inserter = inserter;
        }

        public async Task Consume(ConsumeContext<FileModel> context)
        {
            _logger.LogWarning($"Look! I've got a new file: {context.Message.FileName}, " +
                               $"\nbytes[] = {context.Message.File}\n");

            try
            {
                await using var memoryStream = new MemoryStream(context.Message.File);
                using var streamReader = new StreamReader(memoryStream);
                var xmlSerializer = new XmlSerializer(typeof(TransactionModel));
                var transaction = (TransactionModel) xmlSerializer.Deserialize(streamReader);

                var configuration = _hostBuilderContext.Configuration;
            
                if (transaction != null)
                {
                    transaction.FileName = context.Message.FileName;
                    await _inserter.Insert(
                        new SqlConnection(configuration.GetConnectionString("MSSQLSERVERConnection")),
                        transaction);
                    _logger.LogInformation($"Inserted successfully!");
                }
                else
                {
                    _logger.LogWarning($"Wrong transaction!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}