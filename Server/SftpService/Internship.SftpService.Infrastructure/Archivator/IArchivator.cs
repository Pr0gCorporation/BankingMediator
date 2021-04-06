using System;
using System.Collections.Generic;
using System.Text;

namespace Internship.SftpService.Infrastructure.Archivator
{
    public interface IArchivator
    {
        byte[] ZipArchivation(string filename, byte[] originalFileBytes);
    }
}
