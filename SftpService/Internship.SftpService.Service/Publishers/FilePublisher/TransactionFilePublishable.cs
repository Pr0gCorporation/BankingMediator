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
        
        public async void PublishByOne(IEnumerable<FileDto> files)
        {
            await PublishFiles(files);
        }
        
        public async void PublishAll(IEnumerable<FileDto> files)
        {
            await _publishEndpoint.Publish(files);
        }

        private async Task PublishFiles(IEnumerable<FileDto> files)
        {
            foreach (var file in files)
            {
                await _publishEndpoint.Publish(file);
            }
        }
    }
}