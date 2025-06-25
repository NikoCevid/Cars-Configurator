using Dao.Models;

namespace Dao.Interfaces
{
    public interface IConfigurationCarComponentRepository
    {
        Task<List<ConfigurationCarComponent>> GetByConfigurationIdAsync(int configId);
        Task AddAsync(ConfigurationCarComponent item);
        Task DeleteByConfigurationIdAsync(int configId);
    }
}
