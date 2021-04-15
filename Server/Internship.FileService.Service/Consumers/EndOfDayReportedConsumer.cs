using Internship.FileService.Infrastructure.FileModels.Report;
using Internship.FileService.Infrastructure.SerializerFactoryMethod;
using Internship.Shared.Events;
using Internship.Shared.Events.Report;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Internship.FileService.Service.Consumers
{
    public class EndOfDayReportedConsumer : IConsumer<EndOfDayReportedEvent>
    {
        private readonly ILogger<EndOfDayReportedConsumer> _logger;
        private readonly IBus _bus;
        private readonly FileSerializer _fileSerializer;

        public EndOfDayReportedConsumer(ILogger<EndOfDayReportedConsumer> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
            _fileSerializer = new XmlFileSerializer();
        }

        public async Task Consume(ConsumeContext<EndOfDayReportedEvent> context)
        {
            EndOfDayReportXmlFile endOfDayReportXmlFile = MapReportEventToXmlFileType(context.Message);
            var endOfDayReportXmlFileBytes = await _fileSerializer.Serialize(endOfDayReportXmlFile);
            _logger.LogInformation("New end of day report consumed: " + context.MessageId);

            var outgoingXmlReportFile = new OutgoingFileEvent()
            {
                FileName = "EndOfDayReport_" + context.Message.Date.Month + 
                    "." + context.Message.Date.Day + "_" + RandomNumbers() + ".xml",
                File = endOfDayReportXmlFileBytes
            };

            _logger.LogInformation("File end of day report published: " + outgoingXmlReportFile.FileName);
            await _bus.Publish(outgoingXmlReportFile);
        }

        private static EndOfDayReportXmlFile MapReportEventToXmlFileType(EndOfDayReportedEvent endOfDayReportedEvent)
        {
            return new EndOfDayReportXmlFile()
            {
                Date = endOfDayReportedEvent.Date,
                Reports = endOfDayReportedEvent.AccountReports.Select(report =>
                {
                    return new AccountReportXmlFile()
                    {
                        IBAN = report.IBAN,
                        OpeningBalance = report.OpeningBalance,
                        ClosingBalance = report.ClosingBalance,
                        BalanceMutations = report.BalanceMutations.Select(mutation =>
                        {
                            return new BalanceMutationXmlFile()
                            {
                                DebtorIBAN = mutation.DebtorIBAN,
                                CreditorIBAN = mutation.CreditorIBAN,
                                Amount = mutation.Amount,
                                OriginalReference = mutation.OriginalReference
                            };
                        }).ToList()
                    };
                }).ToList()
            };
        }

        private static string RandomNumbers()
        {
            return new Random().Next(700).ToString();
        }
    }
}
