using Cars.Services.Interfaces;
using Dao.Interfaces;
using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Cars.Services
{
    public class CarComponentService : ICarComponentService
    {
        private readonly ICarComponentRepository _repo;
        private readonly CarsContext _context;

        public CarComponentService(ICarComponentRepository repo, CarsContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<List<CarComponent>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<CarComponent?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task AddAsync(CarComponent component) => await _repo.AddAsync(component);

        public async Task UpdateAsync(CarComponent component) => await _repo.UpdateAsync(component);

        public async Task DeleteAsync(int id)
        {
            var component = await _context.CarComponents
                .Include(c => c.CarComponentCompatibilityCarComponentId1Navigations)
                .Include(c => c.CarComponentCompatibilityCarComponentId2Navigations)
                .Include(c => c.ConfigurationCarComponents)
                .Include(c => c.UserConfigurations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (component == null)
                throw new InvalidOperationException("Komponenta nije pronađena.");

            // Briši povezane entitete ručno
            _context.CarComponentCompatibilities.RemoveRange(component.CarComponentCompatibilityCarComponentId1Navigations);
            _context.CarComponentCompatibilities.RemoveRange(component.CarComponentCompatibilityCarComponentId2Navigations);
            _context.ConfigurationCarComponents.RemoveRange(component.ConfigurationCarComponents);
            _context.UserConfigurations.RemoveRange(component.UserConfigurations);

            _context.CarComponents.Remove(component);
            await _context.SaveChangesAsync();
        }


        public async Task<List<CarComponent>> SearchAsync(string? name, int page, int pageSize)
        {
            var query = _context.CarComponents.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name));

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
