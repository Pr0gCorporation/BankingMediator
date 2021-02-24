using System.IO;
using System.Threading.Tasks;

namespace Internship.FileService.Service.Converters
{
    public class ByteArrayToStreamReaderConverter : IByteConvertable
    {
        public async Task<StreamReader> Convert(byte[] input)
        {
            await using var memoryStream = new MemoryStream(input);
            using var streamReader = new StreamReader(memoryStream);
            return streamReader;
        }
    }
}