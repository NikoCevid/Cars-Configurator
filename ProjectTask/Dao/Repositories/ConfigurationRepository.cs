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
         await _context.Configurations
             .Include(c => c.ConfigurationCarComponents)
             .ThenInclude(cc => cc.CarComponent)
             .Include(c => c.User)
             .FirstOrDefaultAsync(c => c.Id == id);


        public async Task AddAsync(Configuration config)
        {
            _context.Configurations.Add(config);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var configuration = await _context.Configurations
                .Include(c => c.ConfigurationCarComponents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (configuration == null)
                throw new Exception("Configuration not found");

            // Ručno briši povezane ConfigurationCarComponent zapise
            _context.ConfigurationCarComponents.RemoveRange(configuration.ConfigurationCarComponents);

            // Zatim briši samu konfiguraciju
            _context.Configurations.Remove(configuration);

            await _context.SaveChangesAsync();
        }


        public async Task<List<Configuration>> SearchAsync(string? query, int page, int pageSize)
        {
            var configs = _context.Configurations
                .Include(c => c.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                configs = configs.Where(c =>
                    c.User.Username.Contains(query) ||
                    c.Id.ToString().Contains(query) ||
                    c.CreationDate.ToString().Contains(query));
            }

            return await configs
                .OrderByDescending(c => c.CreationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
