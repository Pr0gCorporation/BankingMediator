using System.Collections.Generic;
using Internship.SftpService.Service.DTOs;

namespace Internship.SftpService.Service.FileHandlers
{
    public interface IFileReadable
    {
        /// <summary>
        /// Read all the files in a specified directory as Enumerable of FileDto
        /// </summary>
        /// <returns>IEnumerable of FileDto</returns>
        IEnumerable<FileDto> ReadAllFiles(string path);
        /// <summary>
        /// Read one file, use the full path. Example dir/foo/file.ext
        /// </summary>
        /// <returns>Single FileDto instance</returns>
        FileDto ReadFile(string path);
    }
}