using Cars.Services.Interfaces;
using Dao.Interfaces;
using Dao.Models;

namespace Cars.Services
{
    public class CarComponentService : ICarComponentService
    {
        private readonly ICarComponentRepository _repo;

        public CarComponentService(ICarComponentRepository repo) => _repo = repo;

        public async Task<List<CarComponent>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<CarComponent?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task AddAsync(CarComponent component) => await _repo.AddAsync(component);
        public async Task UpdateAsync(CarComponent component) => await _repo.UpdateAsync(component);
        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}
