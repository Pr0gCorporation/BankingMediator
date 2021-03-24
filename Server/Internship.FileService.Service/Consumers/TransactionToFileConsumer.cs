using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly IFileRepository _repository;
        private readonly IBus _publishEndpoint;

        public TransactionToFileConsumer(ILogger<TransactionToFileConsumer> logger,
          IFileRepository inserter, IBus publishEndpoint)
        {
            _logger = logger;
            _repository = inserter;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<TransactionToFileDto> context)
        {
            int fileId = await _repository.GetNextPrimaryKey();
            var transactionFileModel = TransactionDtoToFileModel(context.Message, fileId);
            var xmlTransactionBytes = await SerializeFileModelToBytes(transactionFileModel);

            const bool isIncomingTransaction = false;

            var fileName = GenerateFileName(
              transactionFileModel.FileId.ToString(),
              transactionFileModel.Transactions.First().Creditor.BankId);

            try
            {
                await _repository.Add(
                  DateTime.Now, isIncomingTransaction,
                  fileName,
                  xmlTransactionBytes);

                _logger.LogInformation($"File {context.MessageId} inserted successfully!");

                await _publishEndpoint.Publish(new OutgoingFileDto()
                {
                    FileName = fileName,
                    File = xmlTransactionBytes
                });

                _logger.LogInformation($"File {context.MessageId} sent to SFTP successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private XMLTransactionFile TransactionDtoToFileModel(TransactionToFileDto fileDto, int fileId)
        {
            return new XMLTransactionFile()
            {
                FileId = fileId,
                Date = fileDto.Date,
                Transactions = new List<TransactionToFile>() {
                    new TransactionToFile() {
                      Debtor = new AccountToFile() {
                          FirstName = fileDto.DebtorFirstName,
                          LastName = fileDto.DebtorLastName,
                          AccountNumber = fileDto.DebtorAccountNumber,
                          BankId = fileDto.DebtorBankId
                        },
                        Creditor = new AccountToFile() {
                          FirstName = fileDto.CreditorFirstName,
                          LastName = fileDto.CreditorLastName,
                          AccountNumber = fileDto.CreditorAccountNumber,
                          BankId = fileDto.CreditorBankId
                        },
                        Amount = fileDto.Amount,
                        EndToEndId = fileDto.TransactionId
                    }
                }
            };
        }

        private async Task<byte[]> SerializeFileModelToBytes(XMLTransactionFile transactionFileModel)
        {
            var serializer = new XmlSerializer(transactionFileModel.GetType());

            string xmlTransactionString = await GetXML(serializer, transactionFileModel);

            return Encoding.ASCII.GetBytes(xmlTransactionString);
        }

        private async Task<string> GetXML(XmlSerializer serializer, XMLTransactionFile transactionFileModel)
        {
            using
            var memoryStream = new MemoryStream();
            serializer.Serialize(memoryStream, transactionFileModel);

            memoryStream.Position = 0;
            return await new StreamReader(memoryStream).ReadToEndAsync();
        }

        private string GenerateFileName(string a, string b)
        {
            var random = new Random();
            return $"{a}_{b}_{random.Next(random.Next(21532))}.xml";
        }
    }
}