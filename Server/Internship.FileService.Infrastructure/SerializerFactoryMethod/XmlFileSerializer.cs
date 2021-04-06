using Internship.FileService.Infrastructure.Serializers;

namespace Internship.FileService.Infrastructure.SerializerFactoryMethod
{
    public class XmlFileSerializer : FileSerializer
    {
        public override IFileSerializable<T> CreateSerializer<T>()
        {
            return new XmlSerializer<T>();
        }
    }
}
