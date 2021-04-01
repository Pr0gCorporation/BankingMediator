using Internship.FileService.Domain.Models.Transaction;
using Internship.Shared.DTOs.Transaction;
using MassTransit;
using Newtonsoft.Json;
using ServiceStack.Text;
using System.Collections.Generic;
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
            IncomingTransactionDto incomingTransaction;
            using (StringReader stringReader = new StringReader(StringTransactionFromByteArray))

            switch (fileExtention)
            {
                case ".xml":
                        System.Xml.Serialization.XmlSerializer xmlSerializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(TransactionFileModel));
                        incomingTransaction = 
                            TransactionFileToIncomingModel((TransactionFileModel)xmlSerializer.Deserialize(stringReader));
                    break;
                case ".json":
                        incomingTransaction =
                            TransactionFileToIncomingModel(
                                JsonConvert.DeserializeObject<TransactionFileModel>(stringReader.ReadToEnd()));
                    break;
                case ".csv":
                        IEnumerable<TransactionCSVFile> deserializedTransactions =
                            CsvSerializer.DeserializeFromString
                                <IEnumerable<TransactionCSVFile>>(stringReader.ReadToEnd());

                        incomingTransaction = TransactionCSVFileToIncomingModel(deserializedTransactions.First());
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

        private IncomingTransactionDto TransactionCSVFileToIncomingModel(TransactionCSVFile transactionCSVFile)
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
