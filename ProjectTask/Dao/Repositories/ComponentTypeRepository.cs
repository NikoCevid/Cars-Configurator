using Dao.Interfaces;
using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Repositories
{
    public class ComponentTypeRepository : IComponentTypeRepository
    {
        private readonly CarsContext _context;

        public ComponentTypeRepository(CarsContext context)
        {
            _context = context;
        }

        public async Task<List<ComponentType>> GetAllAsync()
        {
            return await _context.ComponentTypes.ToListAsync();
        }

        public async Task<ComponentType?> GetByIdAsync(int id)
        {
            return await _context.ComponentTypes.FindAsync(id);
        }

        public async Task AddAsync(ComponentType type)
        {
            _context.ComponentTypes.Add(type);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ComponentType type)
        {
            _context.ComponentTypes.Update(type);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var type = await _context.ComponentTypes.FindAsync(id);
            if (type != null)
            {
                _context.ComponentTypes.Remove(type);
                await _context.SaveChangesAsync();
            }
        }
    }
}
