using Cars.Services.Interfaces;
using Dao.Interfaces;
using Dao.Models;

namespace Cars.Services
{
    public class ComponentTypeService : IComponentTypeService
    {
        private readonly IComponentTypeRepository _repo;

        public ComponentTypeService(IComponentTypeRepository repo) => _repo = repo;

        public async Task<List<ComponentType>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<ComponentType?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task AddAsync(ComponentType type) => await _repo.AddAsync(type);
        public async Task UpdateAsync(ComponentType type) => await _repo.UpdateAsync(type);
        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<List<ComponentType>> SearchAsync(string? query, int page, int pageSize)
    => await _repo.SearchAsync(query, page, pageSize);

    }
}
