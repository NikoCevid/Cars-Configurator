using Dao.Interfaces;
using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Repositories
{
    public class CarComponentRepository : ICarComponentRepository
    {
        private readonly CarsContext _context;

        public CarComponentRepository(CarsContext context)
        {
            _context = context;
        }

        public async Task<List<CarComponent>> GetAllAsync()
        {
            return await _context.CarComponents
                .Include(c => c.ComponentType) 
                .ToListAsync();
        }

        public async Task<CarComponent?> GetByIdAsync(int id)
        {
            return await _context.CarComponents
                .Include(c => c.ComponentType)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(CarComponent component)
        {
            _context.CarComponents.Add(component);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarComponent component)
        {
            _context.CarComponents.Update(component);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var component = await _context.CarComponents.FindAsync(id);
            if (component != null)
            {
                _context.CarComponents.Remove(component);
                await _context.SaveChangesAsync();
            }
        }
    }
}
