using System.Threading.Tasks;
using Internship.SftpService.Service.FileActions.FileReader;
using Internship.SftpService.Service.Publishers.FilePublisher;
using Quartz;

namespace Internship.SftpService.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class DownloadPublishFilesJob : IJob
    {
        private readonly TransactionFilePublisher _publisher;
        private readonly ReadFilesFromSftpServer _reader;

        public DownloadPublishFilesJob(TransactionFilePublisher publisher, 
            ReadFilesFromSftpServer reader)
        {
            _publisher = publisher;
            _reader = reader;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var files = _reader.DownloadAllFiles();
            _publisher.PublishFiles(files);
            
            return Task.CompletedTask;
        }
    }
}