using Internship.FileService.Domain.Models.Transaction;
using Internship.Shared.DTOs.Transaction;
using MassTransit;
using Newtonsoft.Json;
using System.IO;
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
            TransactionFileModel transactionToFile;
            using (StringReader stringReader = new StringReader(StringTransactionFromByteArray))

            switch (fileExtention)
            {
                case ".xml":
                        System.Xml.Serialization.XmlSerializer xmlSerializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(TransactionFileModel));
                        transactionToFile = (TransactionFileModel)xmlSerializer.Deserialize(stringReader);
                    break;
                case ".json":
                        transactionToFile =
                            JsonConvert.DeserializeObject<TransactionFileModel>(stringReader.ReadToEnd());
                    break;
                case ".csv":
                        // todo: add logic to deserialize from csv
                        transactionToFile = null;
                    break;
                default:
                    transactionToFile = null;
                    break;
            }

            if (transactionToFile is not null)
                await _publishEndpoint.Publish(TransactionFileToIncomingModel(transactionToFile));
        }

        private IncomingTransactionDto TransactionFileToIncomingModel(TransactionFileModel TransactionFile)
        {
            return new IncomingTransactionDto()
            {
                DebtorFirstName = TransactionFile.Transactions.First().Debtor.FirstName,
                DebtorLastName = TransactionFile.Transactions.First().Debtor.LastName,
                DebtorAccountNumber = TransactionFile.Transactions.First().Debtor.AccountNumber,
                DebtorBankId = TransactionFile.Transactions.First().Debtor.BankId,
                CreditorFirstName = TransactionFile.Transactions.First().Creditor.FirstName,
                CreditorLastName = TransactionFile.Transactions.First().Creditor.LastName,
                CreditorAccountNumber = TransactionFile.Transactions.First().Creditor.AccountNumber,
                CreditorBankId = TransactionFile.Transactions.First().Creditor.BankId,
                Amount = TransactionFile.Transactions.First().Amount,
                TransactionId = TransactionFile.Transactions.First().EndToEndId
            };
        }
    }
}
