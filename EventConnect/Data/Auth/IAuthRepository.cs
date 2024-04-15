using EventConnect.Entities;
using EventConnect.Models.ServiceResponce;

namespace EventConnect.Data.Auth;

public interface IAuthRepository
{
    Task<ServiceResponse<string>> Register(User user, string password);
    Task<ServiceResponse<string>> Login(string email, string password);
    Task<bool> UserExists(string email);
    Task<User> GetUser(string email);
    
    Task<List<User>> GetAllUsersWithIds(List<string> userIds);
    
   
}
