using Cars.Services.Interfaces;
using Dao.Interfaces;
using Dao.Models;

namespace Cars.Services
{
    public class CarComponentCompatibilityService : ICarComponentCompatibilityService
    {
        private readonly ICarComponentCompatibilityRepository _repo;

        public CarComponentCompatibilityService(ICarComponentCompatibilityRepository repo) => _repo = repo;

        public async Task<List<CarComponentCompatibility>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<CarComponentCompatibility?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task AddAsync(CarComponentCompatibility compatibility) => await _repo.AddAsync(compatibility);
        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}
