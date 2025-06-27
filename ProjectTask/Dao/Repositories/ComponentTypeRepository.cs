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
            var type = await _context.ComponentTypes
                .Include(t => t.CarComponents)
                    .ThenInclude(c => c.ConfigurationCarComponents)
                .Include(t => t.CarComponents)
                    .ThenInclude(c => c.UserConfigurations)
                .Include(t => t.CarComponents)
                    .ThenInclude(c => c.CarComponentCompatibilityCarComponentId1Navigations)
                .Include(t => t.CarComponents)
                    .ThenInclude(c => c.CarComponentCompatibilityCarComponentId2Navigations)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (type == null)
                throw new Exception("Component type not found");

            foreach (var component in type.CarComponents)
            {
                _context.ConfigurationCarComponents.RemoveRange(component.ConfigurationCarComponents);
                _context.UserConfigurations.RemoveRange(component.UserConfigurations);

                _context.CarComponentCompatibilities.RemoveRange(component.CarComponentCompatibilityCarComponentId1Navigations);
                _context.CarComponentCompatibilities.RemoveRange(component.CarComponentCompatibilityCarComponentId2Navigations);
            }

            _context.CarComponents.RemoveRange(type.CarComponents);
            _context.ComponentTypes.Remove(type);

            await _context.SaveChangesAsync();
        }



        public async Task<List<ComponentType>> SearchAsync(string? query, int page, int pageSize)
        {
            var types = _context.ComponentTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                types = types.Where(ct => ct.Name.Contains(query));
            }

            return await types
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
