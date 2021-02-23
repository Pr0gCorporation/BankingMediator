using System;
using System.Threading.Tasks;
using Internship.SftpService.Service.DTOs;
using Internship.SftpService.Service.Publishers;
using Internship.SftpService.Service.SFTPAccess;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace Internship.SftpService.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class DownloadFilesJob : IJob
    {
        private readonly IServerFileDownloadable _downloadable;
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly IFilePublisher _publisher;

        public DownloadFilesJob(IServerFileDownloadable downloadable, HostBuilderContext hostBuilderContext,
            IFilePublisher publisher)
        {
            _downloadable = downloadable;
            _hostBuilderContext = hostBuilderContext;
            _publisher = publisher;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var configuration = _hostBuilderContext.Configuration;
            _downloadable.Download(configuration.GetValue<string>("PathConfig:DownloadFiles:To"),
                configuration.GetValue<string>("PathConfig:DownloadFiles:From"),
                configuration.GetValue<bool>("PathConfig:DownloadFiles:RemoveAfter"));
            
            _publisher.Publish(configuration.GetValue<string>("PathConfig:DownloadFiles:To"));
            return Task.CompletedTask;
        }
    }
}