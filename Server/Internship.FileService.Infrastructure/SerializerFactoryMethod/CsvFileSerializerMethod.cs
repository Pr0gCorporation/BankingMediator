using Internship.FileService.Infrastructure.Serializers;

namespace Internship.FileService.Infrastructure.SerializerFactoryMethod
{
    public class CsvFileSerializerMethod : FileSerializer
    {
        public override IFileSerializable<T> CreateSerializer<T>()
        {
            return new CsvFileSerializer<T>();
        }
    }
}
