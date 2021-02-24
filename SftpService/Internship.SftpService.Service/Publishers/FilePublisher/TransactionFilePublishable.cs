using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Internship.SftpService.Service.DTOs;
using MassTransit;

namespace Internship.SftpService.Service.Publishers.FilePublisher
{
    public class TransactionFilePublishable : IFilePublishable
    {
        private readonly IBus _publishEndpoint;

        public TransactionFilePublishable(IBus publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        
        public async void PublishByOne(List<FileDto> files)
        {
            await PublishFiles(files);
        }
        
        public async void PublishAll(List<FileDto> files)
        {
            await _publishEndpoint.Publish(files);
        }

        private async Task PublishFiles(List<FileDto> files)
        {
            if (files.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(files));
            
            foreach (var file in files)
            {
                await _publishEndpoint.Publish(file);
            }
        }
    }
}