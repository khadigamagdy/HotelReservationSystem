using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservationSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int GuestId { get; set; }

        public Guest Guest { get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public BookingStatus BookingStatus { get; set; }
    }
}