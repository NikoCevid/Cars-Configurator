using Dao.Models;

namespace Cars.Services.Interfaces
{
    public interface IConfigurationCarComponentService
    {
        Task<List<ConfigurationCarComponent>> GetByConfigurationIdAsync(int configId);
        Task AddAsync(ConfigurationCarComponent item);
        Task DeleteByConfigurationIdAsync(int configId);
    }
}
