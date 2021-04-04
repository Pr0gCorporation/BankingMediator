using Internship.FileService.Infrastructure.Serializers;

namespace Internship.FileService.Infrastructure.SerializerFactoryMethod
{
    public abstract class FileSerializer
    {
        public T Deserialize<T>(string dataToSerialize) where T : class
        {
            IFileSerializable<T> serializer = CreateSerializer<T>();
            return serializer.Deserialize(dataToSerialize);
        }

        public abstract IFileSerializable<T> CreateSerializer<T>();
    }
}
