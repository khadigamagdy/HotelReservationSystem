using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.Models.Entities
{
    public class Amenity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<RoomAmenity> RoomAmenities { get; set; }
            = new List<RoomAmenity>();
    }
}