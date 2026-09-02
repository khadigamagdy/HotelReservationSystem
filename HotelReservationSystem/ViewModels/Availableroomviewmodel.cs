using HotelReservationSystem.Models;
using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.ViewModels
{
    public class AvailableRoomsViewModel
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public IEnumerable<Room> AvailableRooms { get; set; } = new List<Room>();
    }
}

