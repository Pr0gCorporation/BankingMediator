using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Internship.SftpService.Service.Jobs
{
    public class SimpleJob : IJob
    {
        async Task IJob.Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Hello, JOb executed");
        }
    }
}