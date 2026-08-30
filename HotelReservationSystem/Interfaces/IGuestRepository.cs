using HotelReservationSystem.Models;

namespace HotelReservationSystem.Interfaces
{
    public interface IGuestRepository
    {
        Task<Guest?> GetByIdAsync(int id);
        Task<Guest?> GetByUserIdAsync(int userId);
        Task AddAsync(Guest guest);
        Task SaveChangesAsync();
    }
}
