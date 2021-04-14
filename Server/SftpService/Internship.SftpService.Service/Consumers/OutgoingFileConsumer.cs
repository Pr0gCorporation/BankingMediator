using System;
using System.IO;
using System.Threading.Tasks;
using Internship.SftpService.Infrastructure.Archivator;
using Internship.SftpService.Service.SFTPActions.UploadFiles;
using Internship.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Internship.SftpService.Service.Consumers
{
    public class OutgoingFileConsumer : IConsumer<OutgoingFileEvent>
    {
        private readonly ILogger<OutgoingFileConsumer> _logger;
        private readonly HostBuilderContext _hostBuilderContext;
        private readonly IServerFileUploadable _uploadable;
        private readonly IArchivator _archivator;
        static object locker = new object();

        public OutgoingFileConsumer(ILogger<OutgoingFileConsumer> logger, IArchivator archivator,
            HostBuilderContext hostBuilderContext, IServerFileUploadable uploadable)
        {
            _logger = logger;
            _archivator = archivator;
            _hostBuilderContext = hostBuilderContext;
            _uploadable = uploadable;
        }

        public async Task Consume(ConsumeContext<OutgoingFileEvent> context)
        {
            _logger.LogInformation($"Received new message: {context.MessageId}, of type {context.GetType()}");

            //try
            //{
            var configuration = _hostBuilderContext.Configuration;

            _logger.LogInformation($"Archiving the file with filename: {context.Message.FileName}, msgId: {context.MessageId}");
            string fileName = context.Message.FileName;
            byte[] fileBytes = context.Message.File;
            string fileNameExtention = Path.GetExtension(fileName);

            byte[] compressedBytes = _archivator.ZipArchivation(fileName, fileBytes);
            string fileNameZip = fileName.Replace(fileNameExtention, ".zip");

            lock (locker)
            {
                _logger.LogInformation($"Uploading the archived file with filename: {context.Message.FileName}, msgId: {context.MessageId}");
                _uploadable.Upload(
                    configuration.GetValue<string>("PathConfig:UploadFiles:To"),
                    compressedBytes, fileNameZip);
                _logger.LogInformation($"The archived file uploaded successfully! From message: {context.MessageId}, where FileName: {fileNameZip}");
            }
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e, $"Upload failed for message {context.MessageId}");
            //    _logger.LogDebug($"File name: {context.Message.FileName}");
            //}
        }
    }
}