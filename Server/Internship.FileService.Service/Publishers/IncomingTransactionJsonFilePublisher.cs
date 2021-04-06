using System.Linq;
using Internship.FileService.Infrastructure.FileModels;
using Internship.FileService.Infrastructure.SerializerFactoryMethod;
using Internship.Shared.DTOs.Transaction;
using MassTransit;

namespace Internship.FileService.Service.Publishers
{
    public class IncomingTransactionJsonFilePublisher : IncomingTransactionFilePublisher
    {
        private readonly IBus _publishEndpoint;
        private readonly FileSerializer _fileSerializer;

        public IncomingTransactionJsonFilePublisher(IBus publishEndpoint)
            : base(publishEndpoint)
        {
            _fileSerializer = new JsonFileSerializer();
        }

        public override IncomingTransactionDto DeserializeToDto(string deserializeFromString)
        {
            return TransactionFileToIncomingModel(
                    _fileSerializer.Deserialize<TransactionFileModel>(deserializeFromString));
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
    }
}
