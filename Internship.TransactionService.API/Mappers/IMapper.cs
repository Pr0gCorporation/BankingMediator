namespace Internship.TransactionService.API.Mappers
{
    public interface IMapper<out T, in U> 
        where T: class, new() 
        where U: class, new()
    {
        T Map(U toMap);
    }
}