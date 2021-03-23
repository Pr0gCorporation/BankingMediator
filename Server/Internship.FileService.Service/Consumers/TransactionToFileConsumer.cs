using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Internship.FileService.Domain.Interfaces;
using Internship.FileService.Domain.Models.Transaction;
using Internship.Shared.DTOs.File;
using Internship.Shared.DTOs.Transaction;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service.Consumers
{
    public class TransactionToFileConsumer : IConsumer<TransactionToFileDto>
    {
        private readonly ILogger<TransactionToFileConsumer> _logger;
        private readonly IFileRepository _inserter;
        private readonly IBus _publishEndpoint;

        public TransactionToFileConsumer(ILogger<TransactionToFileConsumer> logger,
            IFileRepository inserter, IBus publishEndpoint)
        {
            _logger = logger;
            _inserter = inserter;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<TransactionToFileDto> context)
        {
            var transactionFileModel = TransactionDtoToFileModel(context.Message);
            
            var serializer = new XmlSerializer(transactionFileModel.GetType());

            string xmlTransactionString;
            
            await using(var memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, transactionFileModel);

                memoryStream.Position = 0;
                xmlTransactionString = await new StreamReader(memoryStream).ReadToEndAsync();
            }

            var xmlTransactionBytes = Encoding.ASCII.GetBytes(xmlTransactionString);
            
            const bool isIncomingTransaction = false;
            
            var fileName = GenerateFileName(
                transactionFileModel.Creditor.AccountNumber,
                transactionFileModel.Debtor.AccountNumber,
                transactionFileModel.Date);
            
            try
            {
                await _inserter.Add(
                    DateTime.Now, isIncomingTransaction,
                    fileName, 
                    xmlTransactionBytes);
                
                _logger.LogInformation($"Transaction {context.MessageId} inserted successfully!");

                await _publishEndpoint.Publish(new OutgoingFileDto()
                {
                    FileName = fileName,
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
        
        private TransactionToFile TransactionDtoToFileModel(TransactionToFileDto fileDto)
        {
            return new()
            {
                Debtor = new AccountToFile()
                {
                    FirstName = fileDto.DebtorFirstName,
                    LastName = fileDto.DebtorLastName,
                    AccountNumber = fileDto.DebtorAccountNumber,
                    BankId = fileDto.DebtorBankId
                },
                Creditor = new AccountToFile()
                {
                    FirstName = fileDto.CreditorFirstName,
                    LastName = fileDto.CreditorLastName,
                    AccountNumber = fileDto.CreditorAccountNumber,
                    BankId = fileDto.CreditorBankId
                },
                Amount = fileDto.Amount,
                Date = fileDto.Date,
                TransactionId = fileDto.TransactionId
            };
        }

        private string GenerateFileName(string creditor, string debtor, DateTime date)
        {
            var random = new Random();
            return $"{creditor}_{debtor}_{random.Next(random.Next(21532))}.xml";
        }
    }
}