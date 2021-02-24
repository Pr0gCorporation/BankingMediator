using System;
using System.Collections.Generic;
using System.IO;
using Internship.SftpService.Service.DTOs;

namespace Internship.SftpService.Service.FileHandlers
{
    public class ReadXmlFiles : IFileReadable
    {
        public IEnumerable<FileDto> ReadAllFiles(string path)
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

        public FileDto ReadFile(string path)
        {
            return new FileDto
            {
                File = File.ReadAllBytes(path),
                Name = Path.GetFileName(path),
                Date = DateTime.Now
            };
        }
    }
}