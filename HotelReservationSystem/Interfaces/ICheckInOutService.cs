using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Interfaces
{
    public interface ICheckInOutService
    {
        Task<IEnumerable<Reservation>> GetReservationsForCheckInAsync();

        Task<IEnumerable<Reservation>> GetReservationsForCheckOutAsync();

        Task<bool> CheckInAsync(int reservationId, int userId);

        Task<(bool Success, string? ErrorMessage)> CheckOutAsync(
            int reservationId,
            int userId);
    }
}

