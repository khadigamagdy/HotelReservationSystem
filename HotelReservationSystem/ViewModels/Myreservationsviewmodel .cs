using HotelReservationSystem.Models;

namespace HotelReservationSystem.ViewModels
{
    public class MyReservationsViewModel
    {
        public IEnumerable<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
