using System.Threading.Tasks;
using Internship.SftpService.Service.FileActions.FileReader;
using Internship.SftpService.Service.Publishers.FilePublisher;
using Internship.SftpService.Service.SFTPActions.DownloadFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace Internship.SftpService.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class DownloadPublishFilesJob : IJob
    {
        private readonly IServerFileDownloadable _downloadable;
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly TransactionFilePublisher _publisher;
        private readonly ReadXmlFiles _reader;

        public DownloadPublishFilesJob(IServerFileDownloadable downloadable, HostBuilderContext hostBuilderContext,
            TransactionFilePublisher publisher, ReadXmlFiles reader)
        {
            _downloadable = downloadable;
            _hostBuilderContext = hostBuilderContext;
            _publisher = publisher;
            _reader = reader;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var configuration = _hostBuilderContext.Configuration;
            _downloadable.Download(configuration.GetValue<string>("PathConfig:DownloadFiles:To"),
                configuration.GetValue<string>("PathConfig:DownloadFiles:From"),
                configuration.GetValue<bool>("PathConfig:DownloadFiles:RemoveAfter"));
            
            var files = _reader.ReadAllFiles(
                configuration.GetValue<string>("PathConfig:DownloadFiles:To"));
            _publisher.PublishFiles(files);
            
            return Task.CompletedTask;
        }
    }
}