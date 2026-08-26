using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    public class Guest
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string NationalIdOrPassport { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; }

        public List<Reservation> Reservations { get; set; }
            = new List<Reservation>();
    }
}