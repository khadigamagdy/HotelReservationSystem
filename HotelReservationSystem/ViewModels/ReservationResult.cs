using HotelReservationSystem.Models;
using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.ViewModels
{
    public class ReservationResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public Reservation? Reservation { get; set; }

        public static ReservationResult Fail(string message) => new() { Success = false, ErrorMessage = message };
        public static ReservationResult Ok(Reservation reservation) => new() { Success = true, Reservation = reservation };
    }
}
