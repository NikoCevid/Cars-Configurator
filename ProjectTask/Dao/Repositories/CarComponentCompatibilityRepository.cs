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
                .Include(c => c.CarComponentId1Navigation)
                .Include(c => c.CarComponentId2Navigation)
                .ToListAsync();

        public async Task<CarComponentCompatibility?> GetByIdAsync(int id) =>
            await _context.CarComponentCompatibilities
                .Include(cc => cc.CarComponentId1Navigation)
                .Include(cc => cc.CarComponentId2Navigation)
                .FirstOrDefaultAsync(cc => cc.Id == id);

        public async Task AddAsync(CarComponentCompatibility compatibility)
        {
            _context.CarComponentCompatibilities.Add(compatibility);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var compatibility = await _context.CarComponentCompatibilities
                .Include(cc => cc.CarComponentId1Navigation)
                .Include(cc => cc.CarComponentId2Navigation)
                .FirstOrDefaultAsync(cc => cc.Id == id);

            if (compatibility == null)
                throw new Exception("Component compatibility not found");

            _context.CarComponentCompatibilities.Remove(compatibility);
            await _context.SaveChangesAsync();
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

        // Nova metoda za dohvat svih komponenti s njihovim kompatibilnim komponentama
        public async Task<List<CarComponent>> GetAllWithCompatibilitiesAsync()
        {
            return await _context.CarComponents
                .Include(c => c.CarComponentCompatibilityCarComponentId1Navigations)
                    .ThenInclude(cc => cc.CarComponentId2Navigation)
                .ToListAsync();
        }
    }
}
