using Dao.Interfaces;
using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Repositories
{
    public class CarComponentCompatibilityRepository : ICarComponentCompatibilityRepository
    {
        private readonly CarsContext _context;

        public CarComponentCompatibilityRepository(CarsContext context)
        {
            _context = context;
        }

        public async Task<List<CarComponentCompatibility>> GetAllAsync() =>
            await _context.CarComponentCompatibilities
                .Include(c => c.CarComponentId1)
                .Include(c => c.CarComponentId2)
                .ToListAsync();

        public async Task<CarComponentCompatibility?> GetByIdAsync(int id) =>
            await _context.CarComponentCompatibilities.FindAsync(id);

        public async Task AddAsync(CarComponentCompatibility compatibility)
        {
            _context.CarComponentCompatibilities.Add(compatibility);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.CarComponentCompatibilities.FindAsync(id);
            if (item != null)
            {
                _context.CarComponentCompatibilities.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CarComponentCompatibility>> SearchAsync(string? query, int page, int pageSize)
        {
            var data = _context.CarComponentCompatibilities
                .Include(x => x.CarComponentId1Navigation)
                .Include(x => x.CarComponentId2Navigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                data = data.Where(x =>
                    x.CarComponentId1Navigation.Name.Contains(query) ||
                    x.CarComponentId2Navigation.Name.Contains(query));
            }

            return await data
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
