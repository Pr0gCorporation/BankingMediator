namespace Internship.FileService.Infrastructure.Serializers
{
    public interface IFileSerializable<T>
    {
        T Deserialize(string fileString);
    }
}
