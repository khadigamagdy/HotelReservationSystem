using HotelReservationSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models.Entities
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        public int FloorNumber { get; set; }

        public RoomStatus Status { get; set; } = RoomStatus.Available;

        public int RoomTypeId { get; set; }

        public RoomType RoomType { get; set; } = null!;

        public int CreatedByUserId { get; set; }

        public User CreatedByUser { get; set; } = null!;

        public int? LastModifiedByUserId { get; set; }

        public User? LastModifiedByUser { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
            = new List<Reservation>();

        public ICollection<RoomAmenity> RoomAmenities { get; set; }
            = new List<RoomAmenity>();
    }
}