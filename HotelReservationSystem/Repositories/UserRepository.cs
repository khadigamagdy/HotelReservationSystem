using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Data;

namespace HotelReservationSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HotelDbContext _context;

        public UserRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.Include(u => u.Guest)
                                 .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.Include(u => u.Guest)
                                 .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());

        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role) =>
            await _context.Users.Where(u => u.Role == role).ToListAsync();

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();

        public async Task AddAsync(User user) =>
            await _context.Users.AddAsync(user);

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
                _context.Users.Remove(user);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
