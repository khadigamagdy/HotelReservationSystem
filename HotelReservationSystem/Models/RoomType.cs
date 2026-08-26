using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelReservationSystem.Models
{
    public class RoomType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal BasePricePerNight { get; set; }

        [Range(1, 20)]
        public int Capacity { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}