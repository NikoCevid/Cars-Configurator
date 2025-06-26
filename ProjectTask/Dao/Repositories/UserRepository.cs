using Dao.Interfaces;
using Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace Dao.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarsContext _context;

        public UserRepository(CarsContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.FindAsync(id);

        public async Task<User?> GetByUsernameAsync(string username) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();

        public async Task<List<User>> SearchAsync(string? query, int page, int pageSize)
        {
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                users = users.Where(u =>
                    u.FirstName.Contains(query) ||
                    u.LastName.Contains(query) ||
                    u.Username.Contains(query) ||
                    u.Email.Contains(query));
            }

            return await users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
