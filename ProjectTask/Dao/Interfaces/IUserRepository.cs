using Dao.Models;

namespace Dao.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<List<User>> GetAllAsync();
    }
}
