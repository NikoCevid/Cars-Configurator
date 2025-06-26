using Cars.Services.Interfaces;
using Dao.Interfaces;
using Dao.Models;

namespace Cars.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo) => _repo = repo;

        public async Task<User?> GetByIdAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<User?> GetByUsernameAsync(string username)
            => await _repo.GetByUsernameAsync(username);

        public async Task AddAsync(User user)
            => await _repo.AddAsync(user);

        public async Task<List<User>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<List<User>> SearchAsync(string? query, int page, int pageSize)
            => await _repo.SearchAsync(query, page, pageSize); // → DELEGIRAJ NA REPO
    }
}
