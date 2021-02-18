using System.Threading.Tasks;
using Internship.SftpService.Service.SFTPAccess;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Internship.SftpService.Service.Jobs
{
    public class DownloadFilesJob : IJob
    {
        private readonly ILogger<Worker> _logger;
        private readonly IFileDownloadable _downloader;

        public DownloadFilesJob(ILogger<Worker> logger, IFileDownloadable downloader)
        {
            _logger = logger;
            _downloader = downloader;
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            _downloader.Download(@"./downloads/", "upload/out/", true, _logger);
            return Task.CompletedTask;
        }
    }
}