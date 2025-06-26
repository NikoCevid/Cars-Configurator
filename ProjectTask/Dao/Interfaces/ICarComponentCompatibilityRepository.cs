using Dao.Models;

namespace Dao.Interfaces
{
    public interface ICarComponentCompatibilityRepository
    {
        Task<List<CarComponentCompatibility>> GetAllAsync();
        Task<CarComponentCompatibility?> GetByIdAsync(int id);
        Task AddAsync(CarComponentCompatibility compatibility);
        Task DeleteAsync(int id);
        Task<List<CarComponentCompatibility>> SearchAsync(string? query, int page, int pageSize);


    }
}
