using Dao.Models;

namespace Cars.Services.Interfaces
{
    public interface IComponentTypeService
    {
        Task<List<ComponentType>> GetAllAsync();
        Task<ComponentType?> GetByIdAsync(int id);
        Task AddAsync(ComponentType type);
        Task UpdateAsync(ComponentType type);
        Task DeleteAsync(int id);

        Task<List<ComponentType>> SearchAsync(string? query, int page, int pageSize);

    }
}
