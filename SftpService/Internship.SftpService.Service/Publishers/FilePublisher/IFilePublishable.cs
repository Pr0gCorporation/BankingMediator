using System.Collections.Generic;
using Internship.SftpService.Service.DTOs;

namespace Internship.SftpService.Service.Publishers.FilePublisher
{
    public interface IFilePublishable
    {
        /// <summary>
        /// Publish file in a sequence of queries
        /// </summary>
        /// <param name="files"></param>
        void PublishByOne(IEnumerable<FileDto> files);
        /// <summary>
        /// Publish all the files in one query
        /// </summary>
        /// <param name="files"></param>
        void PublishAll(IEnumerable<FileDto> files);
    }
}