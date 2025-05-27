using Dao.Models;

namespace Dao.Interfaces
{
    public interface IComponentTypeRepository
    {
        Task<List<ComponentType>> GetAllAsync();
        Task<ComponentType?> GetByIdAsync(int id);
        Task AddAsync(ComponentType type);
        Task UpdateAsync(ComponentType type);
        Task DeleteAsync(int id);
    }
}
