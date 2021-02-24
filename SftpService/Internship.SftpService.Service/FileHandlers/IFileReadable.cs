using System.Collections.Generic;
using Internship.SftpService.Service.DTOs;

namespace Internship.SftpService.Service.FileHandlers
{
    public interface IFileReadable
    {
        IEnumerable<FileDto> ReadAllFiles(string path);
        FileDto ReadFile(string path);
    }
}