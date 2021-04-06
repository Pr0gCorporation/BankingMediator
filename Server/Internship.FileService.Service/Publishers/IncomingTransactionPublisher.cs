using Internship.FileService.Infrastructure.FileModels;
using Internship.FileService.Infrastructure.SerializerFactoryMethod;
using Internship.Shared.DTOs.Transaction;
using MassTransit;
using System.Linq;

namespace Internship.FileService.Service.Publishers
{
    public class IncomingTransactionPublisher
    {
        private readonly IBus _publishEndpoint;

        public IncomingTransactionPublisher(IBus publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async void PublishIncomingTransaction(byte[] incomingTransactionByteArrayFile, string fileExtention)
        {
            string StringTransactionFromByteArray =
                System.Text.Encoding.Default.GetString(incomingTransactionByteArrayFile);
            IncomingTransactionDto incomingTransaction;
            FileSerializer fileSerializer;

            switch (fileExtention)
            {
                case ".xml":
                    fileSerializer = new XmlFileSerializer();

                    incomingTransaction = TransactionFileToIncomingModel(
                        fileSerializer.Deserialize<TransactionFileModel>(StringTransactionFromByteArray));
                    break;
                case ".json":
                    fileSerializer = new JsonFileSerializer();

                    incomingTransaction =
                        TransactionFileToIncomingModel(
                            fileSerializer.Deserialize<TransactionFileModel>(StringTransactionFromByteArray));
                    break;
                case ".csv":
                    fileSerializer = new CsvFileSerializer();

                    incomingTransaction = TransactionCSVFileToIncomingModel(
                        fileSerializer.Deserialize<TransactionCSVFileModel>(StringTransactionFromByteArray));
                    break;
                default:
                    incomingTransaction = null;
                    break;
            }

            if (incomingTransaction is not null)
                await _publishEndpoint.Publish(incomingTransaction);
        }

        private IncomingTransactionDto TransactionFileToIncomingModel(TransactionFileModel transactionFile)
        {
            return new IncomingTransactionDto()
            {
                DebtorFirstName = transactionFile.Transactions.First().Debtor.FirstName,
                DebtorLastName = transactionFile.Transactions.First().Debtor.LastName,
                DebtorAccountNumber = transactionFile.Transactions.First().Debtor.AccountNumber,
                DebtorBankId = transactionFile.Transactions.First().Debtor.BankId,
                CreditorFirstName = transactionFile.Transactions.First().Creditor.FirstName,
                CreditorLastName = transactionFile.Transactions.First().Creditor.LastName,
                CreditorAccountNumber = transactionFile.Transactions.First().Creditor.AccountNumber,
                CreditorBankId = transactionFile.Transactions.First().Creditor.BankId,
                Amount = transactionFile.Transactions.First().Amount,
                TransactionId = transactionFile.Transactions.First().EndToEndId
            };
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
