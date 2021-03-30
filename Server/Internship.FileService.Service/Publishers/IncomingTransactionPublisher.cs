using Internship.FileService.Domain.Models.Transaction;
using Internship.Shared.DTOs.Transaction;
using MassTransit;
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

        public async void PublishIncomingTransaction(byte[] incomingTransactionXmlFile)
        {
            string StringXmlTransaction = System.Text.Encoding.Default.GetString(incomingTransactionXmlFile);
            XMLTransactionFile transactionToFile;

            using (StringReader stringReader = new StringReader(StringXmlTransaction))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer =
                new System.Xml.Serialization.XmlSerializer(typeof(XMLTransactionFile));
                transactionToFile = (XMLTransactionFile)xmlSerializer.Deserialize(stringReader);
            }

            IncomingTransactionDto incomingTransaction = TransactionFileToIncomingModel(transactionToFile);

            await _publishEndpoint.Publish(incomingTransaction);
        }

        private IncomingTransactionDto TransactionFileToIncomingModel(XMLTransactionFile XMLFile)
        {
            return new IncomingTransactionDto()
            {
                DebtorFirstName = XMLFile.Transactions.First().Debtor.FirstName,
                DebtorLastName = XMLFile.Transactions.First().Debtor.LastName,
                DebtorAccountNumber = XMLFile.Transactions.First().Debtor.AccountNumber,
                DebtorBankId = XMLFile.Transactions.First().Debtor.BankId,
                CreditorFirstName = XMLFile.Transactions.First().Creditor.FirstName,
                CreditorLastName = XMLFile.Transactions.First().Creditor.LastName,
                CreditorAccountNumber = XMLFile.Transactions.First().Creditor.AccountNumber,
                CreditorBankId = XMLFile.Transactions.First().Creditor.BankId,
                Amount = XMLFile.Transactions.First().Amount,
                TransactionId = XMLFile.Transactions.First().EndToEndId
            };
        }
    }
}
