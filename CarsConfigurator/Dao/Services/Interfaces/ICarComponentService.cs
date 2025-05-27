using Dao.Models;

namespace Cars.Services.Interfaces
{
    public interface ICarComponentService
    {
        Task<List<CarComponent>> GetAllAsync();
        Task<CarComponent?> GetByIdAsync(int id);
        Task AddAsync(CarComponent component);
        Task UpdateAsync(CarComponent component);
        Task DeleteAsync(int id);
    }
}
