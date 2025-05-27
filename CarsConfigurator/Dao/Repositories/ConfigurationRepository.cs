using Dao.Interfaces;
using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly CarsContext _context;

        public ConfigurationRepository(CarsContext context)
        {
            _context = context;
        }

        public async Task<List<Configuration>> GetAllAsync() =>
            await _context.Configurations.Include(c => c.User).ToListAsync();

        public async Task<Configuration?> GetByIdAsync(int id) =>
            await _context.Configurations.FindAsync(id);

        public async Task AddAsync(Configuration config)
        {
            _context.Configurations.Add(config);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var config = await _context.Configurations.FindAsync(id);
            if (config != null)
            {
                _context.Configurations.Remove(config);
                await _context.SaveChangesAsync();
            }
        }
    }
}
