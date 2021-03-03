using System;
using System.IO;
using System.Text;
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
    public class TransactionToFileConsumer : IConsumer<Transaction>
    {
        private readonly ILogger<TransactionToFileConsumer> _logger;
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly InsertTransactionToDb _inserter;
        private readonly IBus _publishEndpoint;

        public TransactionToFileConsumer(ILogger<TransactionToFileConsumer> logger,
            HostBuilderContext hostBuilderContext, InsertTransactionToDb inserter, IBus publishEndpoint)
        {
            _logger = logger;
            _hostBuilderContext = hostBuilderContext;
            _inserter = inserter;
            _publishEndpoint = publishEndpoint;
        }
        
        public async Task Consume(ConsumeContext<Transaction> context)
        {
            var serializer = new XmlSerializer(context.Message.GetType());

            string xmlTransactionString;
            
            await using(var memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, context.Message);

                memoryStream.Position = 0;
                xmlTransactionString = await new StreamReader(memoryStream).ReadToEndAsync();
            }

            var xmlTransactionBytes = Encoding.ASCII.GetBytes(xmlTransactionString);
            
            var configuration = _hostBuilderContext.Configuration;

            const bool isIncomingTransaction = false;
            try
            {
                await _inserter.Insert(
                    configuration.GetConnectionString("MYSQLConnection"),
                    DateTime.Now, isIncomingTransaction,
                    GenerateFileName(
                        context.Message.Creditor, 
                        context.Message.Debtor, 
                        context.Message.Date), 
                    xmlTransactionBytes);
                
                _logger.LogInformation($"Transaction {context.Message.Id} inserted successfully!");

                await _publishEndpoint.Publish(new OutgoingFile()
                {
                    FileName = GenerateFileName(
                        context.Message.Creditor, 
                        context.Message.Debtor, 
                        context.Message.Date),
                    File = xmlTransactionBytes
                });
                
                _logger.LogInformation($"Transaction {context.Message.Id} sent to SFTP successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string GenerateFileName(string creditor, string debtor, DateTime date)
        {
            return $"{creditor}_{debtor}_{date.Date.TimeOfDay}.xml";
        }
    }
}