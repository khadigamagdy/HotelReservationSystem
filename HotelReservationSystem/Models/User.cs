using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public int? CreatedByUserId { get; set; }

        public User? CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guest? Guest { get; set; }

        public ICollection<User> CreatedUsers { get; set; }
            = new List<User>();

        public ICollection<Room> CreatedRooms { get; set; }
            = new List<Room>();

        public ICollection<Room> ModifiedRooms { get; set; }
            = new List<Room>();

        public ICollection<Reservation> CreatedReservations { get; set; }
            = new List<Reservation>();

        public ICollection<Reservation> CheckedInReservations { get; set; }
            = new List<Reservation>();

        public ICollection<Reservation> CheckedOutReservations { get; set; }
            = new List<Reservation>();

        public ICollection<Payment> RecordedPayments { get; set; }
            = new List<Payment>();
    }
}