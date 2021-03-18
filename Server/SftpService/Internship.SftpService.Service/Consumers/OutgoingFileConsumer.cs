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
            _logger.LogInformation($"Received new message: {context.MessageId}, of type {context.GetType()}");

            try
            {
                var configuration = _hostBuilderContext.Configuration;


                _logger.LogInformation($"Uploading file with filename: {context.Message.FileName}, msgId: {context.MessageId}");
                _uploadable.Upload(
                    configuration.GetValue<string>("PathConfig:UploadFiles:To"), 
                    context.Message.File, context.Message.FileName);
                
                _logger.LogInformation($"File uploaded successfully! From message: {context.MessageId}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Upload failed for message {context.MessageId}");
                _logger.LogDebug($"File size (bytes): {context.Message.File.Length.ToString()}");
                throw;
            }
        }
    }
}