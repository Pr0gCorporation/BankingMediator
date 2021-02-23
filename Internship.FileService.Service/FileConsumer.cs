using System;
using System.Threading.Tasks;
using Internship.SftpService.Service;
using Internship.SftpService.Service.DTOs;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service
{
    public class FileConsumer : IConsumer<FileDto>
    {
        private readonly ILogger<FileConsumer> _logger;

        public FileConsumer(ILogger<FileConsumer> logger)
        {
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<FileDto> context)
        {
            _logger.LogWarning($"Look! I've got a new file: {context.Message.Name}, " +
                               $"\ndate = {context.Message.Date}, " +
                               $"\nbytes[] = {context.Message.File}");
        }
    }
}