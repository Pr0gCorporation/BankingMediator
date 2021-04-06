using Internship.FileService.Infrastructure.Serializers;

namespace Internship.FileService.Infrastructure.SerializerFactoryMethod
{
    public class CsvFileSerializer : FileSerializer
    {
        public override IFileSerializable<T> CreateSerializer<T>()
        {
            return new CsvFileSerializer<T>();
        }
    }
}
