using HotelReservationSystem.Enums;
using HotelReservationSystem.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.ViewModels
{
    public class RoomCreateViewModel
    {
        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        public int FloorNumber { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        public List<int> AmenityIds { get; set; }
            = new();

        public List<RoomType> RoomTypes { get; set; }
            = new();

        public List<Amenity> Amenities { get; set; }
            = new();
    }

    public class RoomEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        public int FloorNumber { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        public RoomStatus Status { get; set; }

        public List<int> AmenityIds { get; set; }
            = new();

        public List<RoomType> RoomTypes { get; set; }
            = new();

        public List<Amenity> Amenities { get; set; }
            = new();
    }
}
