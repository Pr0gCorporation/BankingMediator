using AutoMapper;
using Internship.Shared.Events;
using Internship.Shared.DTOs.Transaction;
using Internship.TransactionService.Domain.Enums;
using Internship.TransactionService.Domain.Interfaces;
using Internship.TransactionService.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Internship.TransactionService.Service.Consumers
{
    public class IncomingTransactionConsumer : IConsumer<IncomingTransactionDto>
    {
        private readonly ILogger<IncomingTransactionConsumer> _logger;
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IBus _publisher;

        public IncomingTransactionConsumer(ILogger<IncomingTransactionConsumer> logger,
            HostBuilderContext hostBuilderContext, ITransactionRepository transactionRepository,
            IMapper mapper, IBus publisher)
        {
            _logger = logger;
            _hostBuilderContext = hostBuilderContext;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _publisher = publisher;
        }

        public async Task Consume(ConsumeContext<IncomingTransactionDto> context)
        {
            _logger.LogInformation($"Received new message: {context.MessageId}, of type {context.GetType()}");

            const TransactionStatus transactionStatus = TransactionStatus.Created;

            try
            {
                var configuration = _hostBuilderContext.Configuration;

                // Instance to insert
                var transactionModel = _mapper.Map<TransactionModel>(context.Message);
                transactionModel.Incoming = true; // incoming transaction
                _logger.LogInformation($"Incoming transaction: {transactionModel.TransactionId}");

                // Insert to DB
                int transactionPrimaryKey = await _transactionRepository.Add(transactionModel);
                _logger.LogInformation($"Insert to the database the transaction: {transactionModel.TransactionId}");

                // Insert to DB (status of the transaction is created)
                await _transactionRepository.UpdateStatus(new TransactionStatusModel()
                {
                    Status = transactionStatus.ToString("g"),
                    Reason = "",
                    DateStatusChanged = DateTime.Now,
                    TransactionId = transactionPrimaryKey
                });

                _logger.LogInformation($"Insert to the database the transaction status: {transactionModel.TransactionId}, {transactionStatus}");

                // Publish to account service in order to update balance
                await _publisher.Publish(new TransactionRegistredEvent()
                {
                    DebtorIBAN = transactionModel.DebtorAccountNumber,
                    CreditorIBAN = transactionModel.CreditorAccountNumber,
                    Amount = transactionModel.Amount,
                    Reference = transactionModel.TransactionId.ToString()
                });

                _logger.LogInformation($"Publish the transaction, in order to update balance of the accounts: {transactionModel.TransactionId}," +
                    $" Debtor: {transactionModel.DebtorAccountNumber}, Creditor: {transactionModel.CreditorAccountNumber}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Incoming transaction failed for message {context.MessageId}");
                _logger.LogDebug($"Transaction ID: {context.Message.TransactionId}");
                throw;
            }
        }
    }
}
