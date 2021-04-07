using Internship.AccountService.Domain.Interfaces;
using Internship.Shared.Events.Report;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internship.AccountService.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class EndOfDayReportingJob : IJob
    {
        private readonly IBus _bus;
        private readonly IAccountRepository _accountRepository;
        private readonly HostBuilderContext _hostBuilderContext;

        public EndOfDayReportingJob(IBus bus, IAccountRepository accountRepository,
            HostBuilderContext hostBuilderContext)
        {
            _bus = bus;
            _accountRepository = accountRepository;
            _hostBuilderContext = hostBuilderContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var accounts = await _accountRepository.GetAccounts();
            var accountReportsList = new List<List<AccountReport>>
            {
                new List<AccountReport>()
            };

            var configuration = _hostBuilderContext.Configuration;
            int accountsPerFile = configuration.GetValue<int>("ReportConfig:AccountsPerFile");

            foreach (var account in accounts)
            {
                int cashbookId = await _accountRepository.GetCashbookPrimaryKeyByAccountId(account.Id);
                var mutations = await _accountRepository.GetMutationsByCashbookId(cashbookId,
                    DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(-1));

                var balanceMutations = new List<BalanceMutation>();

                foreach (var mutation in mutations)
                {
                    balanceMutations.Add(new BalanceMutation()
                    {
                        DebtorIBAN =
                            await _accountRepository.GetAccountIBANByCashbookId(mutation.CashbookIdDebtor),
                        CreditorIBAN =
                            await _accountRepository.GetAccountIBANByCashbookId(mutation.CashbookIdCreditor),
                        Amount = mutation.Amount,
                        OriginalReference = mutation.OriginReference
                    });
                }

                // The last element (^1 - index operator)
                accountReportsList[^1].Add(new AccountReport()
                {
                    IBAN = account.IBAN,
                    OpeningBalance =
                    await _accountRepository.GetSumOfCashbookRecordsByCashbookIdBeforeDate(cashbookId, DateTime.Now.Date),
                    ClosingBalance =
                    await _accountRepository.GetSumOfCashbookRecordsByCashbookIdBeforeDate(cashbookId, DateTime.Now.Date.AddDays(1)),
                    BalanceMutations = balanceMutations
                });

                if (accountReportsList[^1].Count % accountsPerFile == 0)
                    accountReportsList.Add(new List<AccountReport>());
            }

            foreach (var accountReportsListUnit in accountReportsList)
            {
                await _bus.Publish(new EndOfDayReportedEvent()
                {
                    Date = DateTime.Now,
                    AccountReports = accountReportsListUnit
                });
            }
        }
    }
}
