using Newtonsoft.Json;

namespace Internship.FileService.Infrastructure.Serializers
{
    public class JsonFileSerializer<T> : IFileSerializable<T>
    {
        public T Deserialize(string fileString)
        {
            return JsonConvert.DeserializeObject<T>(fileString);
        }
    }
}
