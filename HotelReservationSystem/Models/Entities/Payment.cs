using HotelReservationSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservationSystem.Models.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }

        public Reservation Reservation { get; set; } = null!;

        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public PaymentMethod Method { get; set; }

        public DateTime PaidAt { get; set; } = DateTime.UtcNow;

        public int? RecordedByUserId { get; set; }

        public User? RecordedByUser { get; set; }
    }
}

