using Cars.Services.Interfaces;
using Dao.Interfaces;
using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Cars.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRepository _repo;

        public ConfigurationService(IConfigurationRepository repo) => _repo = repo;

        public async Task<List<Configuration>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Configuration?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task AddAsync(Configuration config) => await _repo.AddAsync(config);
        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<List<Configuration>> SearchAsync(string? query, int page, int pageSize)
        {
            return await _repo.SearchAsync(query, page, pageSize);
        }


    }
}
