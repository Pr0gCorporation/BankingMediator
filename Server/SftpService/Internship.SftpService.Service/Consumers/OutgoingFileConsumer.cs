using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Internship.SftpService.Service.SFTPActions.UploadFiles;
using Internship.Shared.DTOs.File;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Internship.SftpService.Service.Consumers
{
    public class OutgoingFileConsumer : IConsumer<OutgoingFileDto>
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

        public async Task Consume(ConsumeContext<OutgoingFileDto> context)
        {
            _logger.LogInformation($"Received new message: {context.MessageId}, of type {context.GetType()}");

            try
            {
                var configuration = _hostBuilderContext.Configuration;

                _logger.LogInformation($"Archiving the file with filename: {context.Message.FileName}, msgId: {context.MessageId}");
                string fileName = context.Message.FileName;
                byte[] fileBytes = context.Message.File;
                byte[] compressedBytes;
                string fileNameExtention = Path.GetExtension(fileName);
                string fileNameZip = fileName.Replace(
                    fileNameExtention, ".zip");
                using (var outStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                    {
                        var fileInArchive = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                        using var entryStream = fileInArchive.Open();
                        using var fileToCompressStream = new MemoryStream(fileBytes);
                        fileToCompressStream.CopyTo(entryStream);
                    }
                    compressedBytes = outStream.ToArray();
                }

                _logger.LogInformation($"Uploading the archived file with filename: {context.Message.FileName}, msgId: {context.MessageId}");
                _uploadable.Upload(
                    configuration.GetValue<string>("PathConfig:UploadFiles:To"),
                    compressedBytes, fileNameZip);

                _logger.LogInformation($"The archived file uploaded successfully! From message: {context.MessageId}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Upload failed for message {context.MessageId}");
                _logger.LogDebug($"File size (bytes): {context.Message.File.Length}");
                throw;
            }
        }
    }
}