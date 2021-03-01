using System.Collections.Generic;
using System.IO;
using Internship.FileService.Domain.Models;

namespace Internship.SftpService.Service.FileActions.FileReader
{
    public class ReadXmlFiles
    {
        public List<IncomingFile> ReadAllFiles(string path)
        {
            var fileDtos = new List<IncomingFile>();

            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                if(Path.GetExtension(file) is not ".xml") continue;
                
                fileDtos.Add(
                    new IncomingFile
                    {
                        File = File.ReadAllBytes(file),
                        FileName = Path.GetFileName(file),
                    });
            }

            return fileDtos;
        }

        public IncomingFile ReadFile(string path)
        {
            return new IncomingFile
            {
                File = File.ReadAllBytes(path),
                FileName = Path.GetFileName(path),
            };
        }
    }
}