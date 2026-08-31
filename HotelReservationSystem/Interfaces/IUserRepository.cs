using HotelReservationSystem.Enums;
using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
