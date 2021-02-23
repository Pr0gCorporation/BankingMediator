using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Internship.SftpService.Service.DTOs;
using MassTransit;

namespace Internship.SftpService.Service.Publishers
{
    public class TransactionFilePublisher : IFilePublisher
    {
        private readonly IBus _publishEndpoint;

        public TransactionFilePublisher(IBus publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async void Publish(string path)
        {
            await PublishFiles(ReadFiles(path));
        }

        private IEnumerable<FileDto> ReadFiles(string path)
        {
            var fileDtos = new List<FileDto>();

            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                if(Path.GetExtension(file) is not ".xml") continue;
                
                fileDtos.Add(
                    new FileDto
                    {
                        File = File.ReadAllBytes(file),
                        Name = Path.GetFileName(file),
                        Date = DateTime.Now
                    });
            }

            return fileDtos;
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