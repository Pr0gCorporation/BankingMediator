using Internship.FileService.Infrastructure.FileModels;
using Internship.FileService.Infrastructure.SerializerFactoryMethod;
using Internship.Shared.DTOs.Transaction;
using MassTransit;

namespace Internship.FileService.Service.Publishers
{
    public class IncomingTransactionCsvFilePublisher : IncomingTransactionFilePublisher
    {
        private readonly IBus _publishEndpoint;
        private readonly FileSerializer _fileSerializer;

        public IncomingTransactionCsvFilePublisher(IBus publishEndpoint)
            : base(publishEndpoint)
        {
            _fileSerializer = new CsvFileSerializer();
        }

        public override IncomingTransactionDto DeserializeToDto(string deserializeFromString)
        {
            return TransactionCSVFileToIncomingModel(
                _fileSerializer.Deserialize<TransactionCSVFileModel>(deserializeFromString));
        }

        private IncomingTransactionDto TransactionCSVFileToIncomingModel(TransactionCSVFileModel transactionCSVFile)
        {
            return new IncomingTransactionDto()
            {
                DebtorFirstName = transactionCSVFile.DebtorFirstName,
                DebtorLastName = transactionCSVFile.DebtorLastName,
                DebtorAccountNumber = transactionCSVFile.DebtorAccountNumber,
                DebtorBankId = transactionCSVFile.DebtorBankId,
                CreditorFirstName = transactionCSVFile.CreditorFirstName,
                CreditorLastName = transactionCSVFile.CreditorLastName,
                CreditorAccountNumber = transactionCSVFile.CreditorAccountNumber,
                CreditorBankId = transactionCSVFile.CreditorBankId,
                Amount = transactionCSVFile.Amount,
                TransactionId = transactionCSVFile.EndToEndId
            };
        }
    }
}
