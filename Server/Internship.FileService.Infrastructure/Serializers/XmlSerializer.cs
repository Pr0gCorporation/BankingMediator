using System.IO;

namespace Internship.FileService.Infrastructure.Serializers
{
    public class XmlSerializer<T> : IFileSerializable<T>
    {
        public T Deserialize(string fileString)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(T));

            using StringReader stringReader = new StringReader(fileString);

            return (T)xmlSerializer.Deserialize(stringReader);
        }
    }
}
