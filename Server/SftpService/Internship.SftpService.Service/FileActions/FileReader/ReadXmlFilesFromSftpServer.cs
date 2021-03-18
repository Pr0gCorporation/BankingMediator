using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Internship.FileService.Domain.Models;
using Internship.SftpService.Service.SFTPActions.DownloadFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Internship.SftpService.Service.FileActions.FileReader
{
    public class ReadXmlFilesFromSftpServer
    {
        private readonly IServerFileDownloadable _downloadable;
        private readonly HostBuilderContext _hostBuilderContext;

        public ReadXmlFilesFromSftpServer(IServerFileDownloadable downloadable,
            HostBuilderContext hostBuilderContext)
        {
            _downloadable = downloadable;
            _hostBuilderContext = hostBuilderContext;
        }

        public List<IncomingFile> DownloadAllFiles()
        {
            var incomingFiles = new List<IncomingFile>();
            var configuration = _hostBuilderContext.Configuration;

            var byteArrayFiles = _downloadable.Download(
                configuration.GetValue<string>("PathConfig:DownloadFiles:From"),
                configuration.GetValue<bool>("PathConfig:DownloadFiles:RemoveAfter"));

            foreach (var file in byteArrayFiles)
            {
                var zipByteArray = file;
                var zipStream = new MemoryStream(zipByteArray);
                byte[] xmlByteArrayFile;

                ZipArchive archive = new ZipArchive(zipStream);
                ZipArchiveEntry entry = archive.Entries[0];

                string xmlFileName = entry.FullName;

                var xmlFileStreamFromEntry = entry.Open();
                using var extractedXmlFileStream = new MemoryStream();
                xmlFileStreamFromEntry.CopyTo(extractedXmlFileStream);
                xmlByteArrayFile = extractedXmlFileStream.ToArray();

                incomingFiles.Add(
                    new IncomingFile
                    {
                        File = xmlByteArrayFile,
                        FileName = xmlFileName,
                    });
            }

            return incomingFiles;
        }
    }
}