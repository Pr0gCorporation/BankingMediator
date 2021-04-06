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
            var endOfDayReportXmlFileType = MapReportEventToXmlFileType(context.Message);

            var endOfDayReportXmlFileBytes = await _fileSerializer.Serialize(endOfDayReportXmlFileType);

            // RandomNumbers is for testing, it will be removed after testing is done 
            // (file will be created not at the end but every ** seconds)
            var outgoingXmlReportFile = new OutgoingFileEvent()
            {
                FileName = context.Message.Date.ToString() + RandomNumbers(),
                File = endOfDayReportXmlFileBytes
            };

            await _bus.Publish(outgoingXmlReportFile);
        }

        private static EndOfDayReportXmlFile MapReportEventToXmlFileType(EndOfDayReportedEvent endOfDayReportedEvent)
        {
            return new EndOfDayReportXmlFile()
            {
                Date = endOfDayReportedEvent.Date,
                Reports = endOfDayReportedEvent.Reports.Select(report =>
                {
                    return new ReportXmlFile()
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
                        })
                    };
                })
            };
        }

        private static string RandomNumbers()
        {
            return new Random().Next(7000).ToString();
        }
    }
}
