using System.IO;
using System.Threading.Tasks;

namespace Internship.FileService.Service.Converters
{
    public class ByteArrayToStreamReaderConverter : IByteConvertable
    {
        public StreamReader Convert(byte[] input)
        {
            var memoryStream = new MemoryStream(input);
            var streamReader = new StreamReader(memoryStream);
            return streamReader;
        }
    }
}