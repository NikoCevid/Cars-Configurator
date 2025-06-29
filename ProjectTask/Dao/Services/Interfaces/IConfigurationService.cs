﻿using Dao.Models;

namespace Cars.Services.Interfaces
{
    public interface IConfigurationService
    {
        Task<List<Configuration>> GetAllAsync();
        Task<Configuration?> GetByIdAsync(int id);
        Task AddAsync(Configuration config);
        Task DeleteAsync(int id);

        Task<List<Configuration>> SearchAsync(string? query, int page, int pageSize);

    }
}
