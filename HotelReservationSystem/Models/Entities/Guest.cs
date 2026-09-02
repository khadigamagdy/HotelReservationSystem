using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models.Entities
{
    public class Guest
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string NationalIdOrPassport { get; set; } = string.Empty;

        [Required]
        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        public ICollection<Reservation> Reservations { get; set; }
            = new List<Reservation>();
    }
}
