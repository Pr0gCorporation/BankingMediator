using System.Collections.Generic;
using Internship.SftpService.Service.Models;

namespace Internship.SftpService.Service.FileActions.FileReader
{
    public interface IFileReadable
    {
        /// <summary>
        /// Read all the files in a specified directory as Enumerable of FileDto
        /// </summary>
        /// <returns>IEnumerable of FileDto</returns>
        List<FileModel> ReadAllFiles(string path);
        /// <summary>
        /// Read one file, use the full path. Example dir/foo/file.ext
        /// </summary>
        /// <returns>Single FileDto instance</returns>
        FileModel ReadFile(string path);
    }
}