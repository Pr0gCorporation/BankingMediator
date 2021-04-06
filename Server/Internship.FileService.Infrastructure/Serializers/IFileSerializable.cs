using System.Threading.Tasks;

namespace Internship.FileService.Infrastructure.Serializers
{
    public interface IFileSerializable<T>
    {
        T Deserialize(string fileString);
        Task<byte[]> Serialize(T dataToSerialize);
    }
}
