using Cars.Services.Interfaces;
using Dao.Interfaces;
using Dao.Models;

namespace Cars.Services
{
    public class ConfigurationCarComponentService : IConfigurationCarComponentService
    {
        private readonly IConfigurationCarComponentRepository _repo;

        public ConfigurationCarComponentService(IConfigurationCarComponentRepository repo) => _repo = repo;

        public async Task<List<ConfigurationCarComponent>> GetByConfigurationIdAsync(int configId) =>
            await _repo.GetByConfigurationIdAsync(configId);

        public async Task AddAsync(ConfigurationCarComponent item) => await _repo.AddAsync(item);
        public async Task DeleteByConfigurationIdAsync(int configId) => await _repo.DeleteByConfigurationIdAsync(configId);
    }
}
