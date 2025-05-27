using Dao.Interfaces;
using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Repositories
{
    public class ConfigurationCarComponentRepository : IConfigurationCarComponentRepository
    {
        private readonly CarsContext _context;

        public ConfigurationCarComponentRepository(CarsContext context)
        {
            _context = context;
        }

        public async Task<List<ConfigurationCarComponent>> GetByConfigurationIdAsync(int configId) =>
            await _context.ConfigurationCarComponents
                .Where(x => x.ConfigurationId == configId)
                .Include(x => x.CarComponent)
                .ToListAsync();

        public async Task AddAsync(ConfigurationCarComponent item)
        {
            _context.ConfigurationCarComponents.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByConfigurationIdAsync(int configId)
        {
            var items = _context.ConfigurationCarComponents.Where(x => x.ConfigurationId == configId);
            _context.ConfigurationCarComponents.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
