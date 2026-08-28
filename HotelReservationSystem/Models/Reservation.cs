using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservationSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int GuestId { get; set; }

        public Guest Guest { get; set; } = null!;

        public int RoomId { get; set; }

        public Room Room { get; set; } = null!;

        public int CreatedByUserId { get; set; }

        public User CreatedByUser { get; set; } = null!;

        public int? CheckedInByUserId { get; set; }

        public User? CheckedInByUser { get; set; }

        public int? CheckedOutByUserId { get; set; }

        public User? CheckedOutByUser { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
            = PaymentStatus.Unpaid;

        public BookingStatus BookingStatus { get; set; }
            = BookingStatus.Booked;

        public ICollection<Payment> Payments { get; set; }
            = new List<Payment>();
    }
}