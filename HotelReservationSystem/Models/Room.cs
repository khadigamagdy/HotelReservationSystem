using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public bool IsMaintenance { get; set; }

        public int RoomTypeId { get; set; }

        public RoomType RoomType { get; set; }

        public List<Reservation> Reservations { get; set; }
            = new List<Reservation>();
    }
}