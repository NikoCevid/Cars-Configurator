﻿using Dao.Models;

namespace Cars.Services.Interfaces
{
    public interface ICarComponentCompatibilityService
    {
        Task<List<CarComponentCompatibility>> GetAllAsync();
        Task<CarComponentCompatibility?> GetByIdAsync(int id);
        Task AddAsync(CarComponentCompatibility compatibility);
        Task DeleteAsync(int id);

        Task<List<CarComponentCompatibility>> SearchAsync(string? query, int page, int pageSize);

    }
}
