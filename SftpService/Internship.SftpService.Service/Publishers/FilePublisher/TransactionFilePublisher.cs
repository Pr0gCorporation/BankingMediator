using System.Collections.Generic;
using Internship.SftpService.Domain.Models;
using MassTransit;

namespace Internship.SftpService.Service.Publishers.FilePublisher
{
    public class TransactionFilePublisher
    {
        private readonly IBus _publishEndpoint;

        public TransactionFilePublisher(IBus publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async void PublishFiles(IEnumerable<FileModel> files)
        {
            if (files is null) return;
            
            foreach (var file in files)
            {
                await _publishEndpoint.Publish(file);
            }
        }
    }
}