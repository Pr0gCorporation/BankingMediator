using System.Threading.Tasks;
using Internship.SftpService.Service.SFTPAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Internship.SftpService.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class DownloadFilesJob : IJob
    {
        private readonly IServerFileDownloadable _downloadable;
        private readonly HostBuilderContext _hostBuilderContext;

        public DownloadFilesJob(IServerFileDownloadable downloadable, HostBuilderContext hostBuilderContext)
        {
            _downloadable = downloadable;
            _hostBuilderContext = hostBuilderContext;
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            var configuration = _hostBuilderContext.Configuration;
            _downloadable.Download(configuration.GetValue<string>("PathConfig:DownloadFiles:To"), 
                configuration.GetValue<string>("PathConfig:DownloadFiles:From"), 
                configuration.GetValue<bool>("PathConfig:DownloadFiles:RemoveAfter"));
            return Task.CompletedTask;
        }
    }
}
