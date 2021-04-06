using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Internship.SftpService.Service.SFTPActions.DownloadFiles;
using Internship.Shared.DTOs.File;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Internship.SftpService.Service.FileActions.FileReader
{
    public class ReadFilesFromSftpServer
    {
        private readonly IServerFileDownloadable _downloadable;
        private readonly HostBuilderContext _hostBuilderContext;

        public ReadFilesFromSftpServer(IServerFileDownloadable downloadable,
            HostBuilderContext hostBuilderContext)
        {
            _downloadable = downloadable;
            _hostBuilderContext = hostBuilderContext;
        }

        public List<IncomingFileDto> DownloadAllFiles()
        {
            var incomingFiles = new List<IncomingFileDto>();
            var configuration = _hostBuilderContext.Configuration;

            var zipByteArrayFiles = _downloadable.Download(
                configuration.GetValue<string>("PathConfig:DownloadFiles:From"),
                configuration.GetValue<bool>("PathConfig:DownloadFiles:RemoveAfter"));

            foreach (var file in zipByteArrayFiles)
            {
                var zipByteArray = file;
                var zipStream = new MemoryStream(zipByteArray);
                byte[] byteArrayFile;

                ZipArchive archive = new ZipArchive(zipStream);
                ZipArchiveEntry entry = archive.Entries[0];

                string fileName = entry.FullName;

                var fileStreamFromEntry = entry.Open();
                using var extractedFileStream = new MemoryStream();
                fileStreamFromEntry.CopyTo(extractedFileStream);
                byteArrayFile = extractedFileStream.ToArray();

                incomingFiles.Add(
                    new IncomingFileDto
                    {
                        File = byteArrayFile,
                        FileName = fileName,
                    });
            }

            return incomingFiles;
        }
    }
}