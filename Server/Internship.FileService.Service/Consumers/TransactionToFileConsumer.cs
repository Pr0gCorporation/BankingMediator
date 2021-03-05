using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Internship.FileService.Domain.Models;
using Internship.FileService.Service.DBAccess;
using Internship.Shared;
using Internship.Shared.Files;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service.Consumers
{
    public class TransactionToFileConsumer : IConsumer<TransactionToFile>
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
        
        public async Task Consume(ConsumeContext<TransactionToFile> context)
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
                        context.Message.CreditorAccountNumber, 
                        context.Message.DebtorAccountNumber, 
                        context.Message.Date), 
                    xmlTransactionBytes);
                
                _logger.LogInformation($"Transaction {context.MessageId} inserted successfully!");

                await _publishEndpoint.Publish(new OutgoingFile()
                {
                    FileName = GenerateFileName(
                        context.Message.CreditorAccountNumber, 
                        context.Message.DebtorAccountNumber, 
                        context.Message.Date),
                    File = xmlTransactionBytes
                });
                
                _logger.LogInformation($"Transaction {context.MessageId} sent to SFTP successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string GenerateFileName(string creditor, string debtor, DateTime date)
        {
            var random = new Random();
            return $"{creditor}_{debtor}_{random.Next(random.Next(21532))}.xml";
        }
    }
}