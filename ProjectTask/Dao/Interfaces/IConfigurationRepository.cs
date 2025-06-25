using Dao.Models;

namespace Dao.Interfaces
{
    public interface IConfigurationRepository
    {
        Task<List<Configuration>> GetAllAsync();
        Task<Configuration?> GetByIdAsync(int id);
        Task AddAsync(Configuration config);
        Task DeleteAsync(int id);
    }
}
