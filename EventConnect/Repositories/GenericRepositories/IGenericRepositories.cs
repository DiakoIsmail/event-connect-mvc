using EventConnect.Models.ServiceResponce;

namespace EventConnect.Repositories.GenericRepositories;

public interface IGenericRepositories<T> where T: class
{
    Task<T?>CreateAsync(T entity);
    Task<T?> UpdateAsync(T entity);
    Task<T?>DeleteAsync(T entity);
    Task <List<T?>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
    
}