using ServiceStack.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internship.FileService.Infrastructure.Serializers
{
    public class CsvFileSerializer<T> : IFileSerializable<T>
    {
        public T Deserialize(string fileString)
        {
            IEnumerable<T> deserializedTransactions =
                CsvSerializer.DeserializeFromString<IEnumerable<T>>(fileString);

            // this thing could return many (list of) transactions, it needs to think about one.

            return deserializedTransactions.First();
        }

        public async Task<byte[]> Serialize(T fileString)
        {
            throw new System.NotImplementedException();
        }
    }
}
