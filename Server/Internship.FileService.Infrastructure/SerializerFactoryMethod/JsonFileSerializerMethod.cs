using Internship.FileService.Infrastructure.Serializers;

namespace Internship.FileService.Infrastructure.SerializerFactoryMethod
{
    public class JsonFileSerializerMethod : FileSerializer
    {
        public override IFileSerializable<T> CreateSerializer<T>()
        {
            return new JsonFileSerializer<T>();
        }
    }
}
