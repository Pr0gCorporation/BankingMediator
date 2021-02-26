using System;
using System.Collections.Generic;
using System.IO;
using Internship.SftpService.Domain.Models;

namespace Internship.SftpService.Service.FileActions.FileReader
{
    public class ReadXmlFiles
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
                        FileName = Path.GetFileName(file),
                    });
            }

            return fileDtos;
        }

        public FileModel ReadFile(string path)
        {
            return new FileModel
            {
                File = File.ReadAllBytes(path),
                FileName = Path.GetFileName(path),
            };
        }
    }
}