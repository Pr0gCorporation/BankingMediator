using Internship.Shared.DTOs.Transaction;
using MassTransit;

namespace Internship.FileService.Service.Publishers
{
    public abstract class IncomingTransactionFilePublisher
    {
        private readonly IBus _publishEndpoint;

        public IncomingTransactionFilePublisher(IBus publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async void PublishIncomingTransaction(byte[] incomingTransactionByteArrayFile)
        {
            string stringTransactionFromByteArray =
                System.Text.Encoding.Default.GetString(incomingTransactionByteArrayFile);

            var incomingTransaction = DeserializeToDto(stringTransactionFromByteArray);

            if (incomingTransaction is not null)
                await _publishEndpoint.Publish(incomingTransaction);
        }

        public abstract IncomingTransactionDto DeserializeToDto(string deserializeFromString);
    }
}
