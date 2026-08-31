using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.ViewModels
{
    public class CreateReservationViewModel
    {
        [Required]
        public int RoomId { get; set; }

        public string RoomNumber { get; set; } = string.Empty;
        public string RoomTypeName { get; set; } = string.Empty;
        public decimal BasePricePerNight { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-in date")]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-out date")]
        public DateTime CheckOutDate { get; set; }
    }
}