using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Internship.FileService.Domain.Models;
using Internship.FileService.Service.DBAccess;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service.Consumers
{
    public class IncomingTransactionConsumer : IConsumer<IncomingFile>
    {
        private readonly ILogger<IncomingTransactionConsumer> _logger;
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly InsertTransactionToDb _inserter;

        public IncomingTransactionConsumer(ILogger<IncomingTransactionConsumer> logger,
            HostBuilderContext hostBuilderContext, InsertTransactionToDb inserter)
        {
            _logger = logger;
            _hostBuilderContext = hostBuilderContext;
            _inserter = inserter;
        }

        public async Task Consume(ConsumeContext<IncomingFile> context)
        {
            _logger.LogWarning($"Look! I've got a new file: {context.Message.FileName}, " +
                               $"\nbytes[] = {context.Message.File}\n");

            try
            {
                var configuration = _hostBuilderContext.Configuration;

                await _inserter.Insert(
                    configuration.GetConnectionString("MYSQLConnection"),
                    DateTime.Now,  "incoming",
                    context.Message.FileName, 
                    context.Message.File);
                
                _logger.LogInformation($"Inserted successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}