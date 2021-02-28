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
    public class OutgoingPaymentConsumer : IConsumer<Transaction>
    {
        private readonly ILogger<OutgoingPaymentConsumer> _logger;
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly InsertTransactionToDb _inserter;
        
        public OutgoingPaymentConsumer(ILogger<OutgoingPaymentConsumer> logger,
            HostBuilderContext hostBuilderContext, InsertTransactionToDb inserter)
        {
            _logger = logger;
            _hostBuilderContext = hostBuilderContext;
            _inserter = inserter;
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
            
            try
            {
                var configuration = _hostBuilderContext.Configuration;

                await _inserter.Insert(
                    configuration.GetConnectionString("MYSQLConnection"),
                    DateTime.Now, "outgoing",
                    GenerateFileName(
                        context.Message.Creditor, 
                        context.Message.Debtor, 
                        context.Message.Date), 
                    xmlTransactionBytes);
                
                _logger.LogInformation($"Inserted successfully!");
                
                //
                // TODO: publish to sftp endpoint
                //
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string GenerateFileName(string creditor, string debtor, DateTime date)
        {
            return $"{creditor}_{debtor}_{date.Date}";
        }
    }
}