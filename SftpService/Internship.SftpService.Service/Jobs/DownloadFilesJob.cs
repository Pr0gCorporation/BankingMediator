using System.Threading.Tasks;
using Internship.SftpService.Service.SFTPAccess;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Internship.SftpService.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class DownloadFilesJob : IJob
    {
        private readonly IServerFileDownloadable _downloadable;

        public DownloadFilesJob(IServerFileDownloadable downloadable)
        {
            _downloadable = downloadable;
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            _downloadable.Download(@"./downloads/", "upload/out/", true);
            return Task.CompletedTask;
        }
    }
}
