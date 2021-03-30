using System;
using System.Threading.Tasks;
using Internship.AccountService.Domain.Interfaces;
using Internship.AccountService.Domain.Models;
using Internship.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Internship.AccountService.Service.Consumers
{
    public class UpdateAccountBalanceConsumer : IConsumer<TransactionRegistredEvent>
    {
        private readonly ILogger<UpdateAccountBalanceConsumer> _logger;
        private readonly IAccountRepository _repository;

        public UpdateAccountBalanceConsumer(ILogger<UpdateAccountBalanceConsumer> logger,
             IAccountRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<TransactionRegistredEvent> context)
        {
            _logger.LogInformation($"Received new message: {context.MessageId}, of type {context.GetType()}");

            var accountBalanceDto = context.Message;

            try
            {
                // Get all the necessary data
                var debtorAccountId = await _repository.GetAccountPrimaryKeyByIBAN(accountBalanceDto.DebtorIBAN);
                var creditorAccountId = await _repository.GetAccountPrimaryKeyByIBAN(accountBalanceDto.CreditorIBAN);
                var debtorCashbookId = await _repository.GetCashbookPrimaryKeyByAccountId(debtorAccountId);
                var creditorCashbookId = await _repository.GetCashbookPrimaryKeyByAccountId(creditorAccountId);

                var time = DateTime.Now;

                // Insert the cashbook record about decreased amount of the debtor
                _ = await _repository.InsertCashbookRecord(new CashbookRecordModel()
                {
                    CashbookId = debtorCashbookId,
                    Date = time,
                    Amount = -1 * accountBalanceDto.Amount,
                    OriginReference = accountBalanceDto.Reference
                });

                // Insert the cashbook record about increased amount of the creditor
                _ = await _repository.InsertCashbookRecord(new CashbookRecordModel()
                {
                    CashbookId = creditorCashbookId,
                    Date = time,
                    Amount = accountBalanceDto.Amount,
                    OriginReference = accountBalanceDto.Reference
                });

                // Update the total balance in the cashbook table each of the accounts
                var debtorResultantBalance = await _repository.GetSumOfCashbookRecordsByCashbookId(debtorCashbookId);
                var creditorResultantBalance = await _repository.GetSumOfCashbookRecordsByCashbookId(creditorCashbookId);

                _ = await _repository.UpdateCashbookBalance(debtorCashbookId, debtorResultantBalance);
                _ = await _repository.UpdateCashbookBalance(creditorCashbookId, creditorResultantBalance);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Updating account balance failed {context.MessageId}");
                throw;
            }
        }
    }
}