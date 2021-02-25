using System;
using System.Collections.Generic;
using System.IO;
using Internship.SftpService.Service.Models;

namespace Internship.SftpService.Service.FileActions.FileReader
{
    public class ReadXmlFiles : IFileReadable
    {
        public List<FileModel> ReadAllFiles(string path)
        {
            var fileDtos = new List<FileModel>();

            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                if(Path.GetExtension(file) is not ".xml") continue;
                
                fileDtos.Add(
                    new FileModel
                    {
                        File = File.ReadAllBytes(file),
                        Name = Path.GetFileName(file),
                        Date = DateTime.Now
                    });
            }

            return fileDtos;
        }

        public FileModel ReadFile(string path)
        {
            return new FileModel
            {
                File = File.ReadAllBytes(path),
                Name = Path.GetFileName(path),
                Date = DateTime.Now
            };
        }
    }
}