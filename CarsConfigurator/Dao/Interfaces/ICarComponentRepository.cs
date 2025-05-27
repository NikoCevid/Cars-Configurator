using Dao.Models;

namespace Dao.Interfaces
{
    public interface ICarComponentRepository
    {
        Task<List<CarComponent>> GetAllAsync();
        Task<CarComponent?> GetByIdAsync(int id);
        Task AddAsync(CarComponent component);
        Task UpdateAsync(CarComponent component);
        Task DeleteAsync(int id);
    }
}
