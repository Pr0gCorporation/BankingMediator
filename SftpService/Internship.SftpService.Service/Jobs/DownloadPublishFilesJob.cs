using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Internship.SftpService.Service.DTOs;
using Internship.SftpService.Service.FileActions.FileReader;
using Internship.SftpService.Service.Publishers;
using Internship.SftpService.Service.Publishers.FilePublisher;
using Internship.SftpService.Service.SFTPActions.DownloadFiles;
using MassTransit;
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
        private readonly IFilePublishable _publishable;
        private readonly IFileReadable _reader;

        public DownloadPublishFilesJob(IServerFileDownloadable downloadable, HostBuilderContext hostBuilderContext,
            IFilePublishable publishable, IFileReadable reader)
        {
            _downloadable = downloadable;
            _hostBuilderContext = hostBuilderContext;
            _publishable = publishable;
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
            _publishable.PublishByOne(files);
            
            return Task.CompletedTask;
        }
    }
}