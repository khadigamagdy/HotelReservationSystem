using HotelReservationSystem.Models;
using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetByIdAsync(int id);
        Task<IEnumerable<Reservation>> GetByGuestIdAsync(int guestId);
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut);
        Task<Room?> GetRoomByIdAsync(int roomId);
        Task AddAsync(Reservation reservation);
        Task SaveChangesAsync();
    }
}
