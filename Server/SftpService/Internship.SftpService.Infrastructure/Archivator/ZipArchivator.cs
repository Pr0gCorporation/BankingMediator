using System.IO;
using System.IO.Compression;

namespace Internship.SftpService.Infrastructure.Archivator
{
    public class ZipArchivator : IArchivator
    {
        public byte[] ZipArchivation(string filename, byte[] originalFileBytes)
        {
            using var outStream = new MemoryStream();
            using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
            {
                var fileInArchive = archive.CreateEntry(filename, CompressionLevel.Optimal);
                using var entryStream = fileInArchive.Open();
                using var fileToCompressStream = new MemoryStream(originalFileBytes);
                fileToCompressStream.CopyTo(entryStream);
            }
            return outStream.ToArray();
        }
    }
}
