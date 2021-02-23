using System;
using System.Threading.Tasks;
using Internship.SftpService.Service;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Internship.FileService.Service
{
    public class MessageConsumer: IConsumer<Message>
    {
        private readonly ILogger<MessageConsumer> logger;

        public MessageConsumer(ILogger<MessageConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<Message> context)
        {
            logger.LogWarning($"Look! I've got a new message: {context.Message.Text}");
        }
    }
}