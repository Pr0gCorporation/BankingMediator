using Internship.FileService.Domain.Models.Transaction;
using Internship.Shared.DTOs.Transaction;
using MassTransit;
using System.IO;

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
            TransactionToFile transactionToFile;

            using (StringReader stringReader = new StringReader(StringXmlTransaction))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer =
                new System.Xml.Serialization.XmlSerializer(typeof(TransactionToFile));
                transactionToFile = (TransactionToFile)xmlSerializer.Deserialize(stringReader);
            }

            IncomingTransactionDto incomingTransaction = TransactionFileToIncomingModel(transactionToFile);

            await _publishEndpoint.Publish(incomingTransaction);
        }

        private IncomingTransactionDto TransactionFileToIncomingModel(TransactionToFile fileDto)
        {
            return new IncomingTransactionDto()
            {
                DebtorFirstName = fileDto.Debtor.FirstName,
                DebtorLastName = fileDto.Debtor.LastName,
                DebtorAccountNumber = fileDto.Debtor.AccountNumber,
                DebtorBankId = fileDto.Debtor.BankId,
                CreditorFirstName = fileDto.Creditor.FirstName,
                CreditorLastName = fileDto.Creditor.LastName,
                CreditorAccountNumber = fileDto.Creditor.AccountNumber,
                CreditorBankId = fileDto.Creditor.BankId,
                Amount = fileDto.Amount,
                TransactionId = fileDto.TransactionId
            };
        }
    }
}
