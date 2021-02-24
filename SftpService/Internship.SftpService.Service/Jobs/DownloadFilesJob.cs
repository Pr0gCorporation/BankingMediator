using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Internship.SftpService.Service.DTOs;
using Internship.SftpService.Service.FileHandlers;
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
        private readonly IFileReadable _reader;

        public DownloadFilesJob(IServerFileDownloadable downloadable, HostBuilderContext hostBuilderContext,
            IFilePublisher publisher, IFileReadable reader)
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
            _publisher.PublishAll(files);
            
            return Task.CompletedTask;
        }
    }
}