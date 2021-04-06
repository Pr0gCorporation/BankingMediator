using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Internship.FileService.Infrastructure.Serializers
{
    public class JsonFileSerializer<T> : IFileSerializable<T>
    {
        public T Deserialize(string fileString)
        {
            return JsonConvert.DeserializeObject<T>(fileString);
        }

        public async Task<byte[]> Serialize(T fileString)
        {
            throw new System.NotImplementedException();
        }
    }
}
