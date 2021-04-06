using MassTransit;
using Quartz;
using System.Threading.Tasks;

namespace Internship.AccountService.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class EndOfDayReportingJob : IJob
    {
        IBus _bus;

        public EndOfDayReportingJob(IBus bus)
        {
            _bus = bus;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // 1) 

            await _bus.Publish();
        }
    }
}
