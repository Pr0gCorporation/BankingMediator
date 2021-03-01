using System;
using System.Threading.Tasks;
using Internship.FileService.Domain.Models;
using Internship.SftpService.Service.SFTPActions.UploadFiles;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Internship.SftpService.Service.Consumers
{
    public class OutgoingFileConsumer : IConsumer<OutgoingFile>
    {
        private readonly ILogger<OutgoingFileConsumer> _logger;
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly IServerFileUploadable _uploadable;

        public OutgoingFileConsumer(ILogger<OutgoingFileConsumer> logger,
            HostBuilderContext hostBuilderContext, IServerFileUploadable uploadable)
        {
            _logger = logger;
            _hostBuilderContext = hostBuilderContext;
            _uploadable = uploadable;
        }
        
        public async Task Consume(ConsumeContext<OutgoingFile> context)
        {
            _logger.LogWarning($"Look! I've got a new file: {context.Message.FileName}, " +
                               $"\nbytes[] = {context.Message.File}\n");

            try
            {
                var configuration = _hostBuilderContext.Configuration;

                _uploadable.Upload(
                    configuration.GetValue<string>("PathConfig:UploadFiles:To"), 
                    context.Message.File, context.Message.FileName);
                
                _logger.LogInformation($"Uploaded successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}