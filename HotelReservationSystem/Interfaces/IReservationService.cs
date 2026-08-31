using HotelReservationSystem.Models;
using HotelReservationSystem.ViewModels;

namespace HotelReservationSystem.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<Room>> SearchAvailableRoomsAsync(DateTime checkIn, DateTime checkOut);
        Task<Room?> GetRoomDetailsAsync(int roomId);
        Task<ReservationResult> CreateReservationAsync(int guestId, int userId, int roomId, DateTime checkIn, DateTime checkOut);
        Task<IEnumerable<Reservation>> GetMyReservationsAsync(int guestId);
        Task<Reservation?> GetReservationDetailsAsync(int reservationId, int guestId);
        Task<ReservationResult> CancelReservationAsync(int reservationId, int guestId);
    }
}
