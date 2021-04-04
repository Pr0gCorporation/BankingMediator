using Internship.FileService.Infrastructure.Serializers;

namespace Internship.FileService.Infrastructure.SerializerFactoryMethod
{
    public class XmlFileSerializerMethod : FileSerializer
    {
        public override IFileSerializable<T> CreateSerializer<T>()
        {
            return new XmlSerializer<T>();
        }
    }
}
