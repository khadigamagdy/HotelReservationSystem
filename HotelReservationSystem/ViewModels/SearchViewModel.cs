using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.ViewModels
{
    public class SearchViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-in date")]
        public DateTime CheckInDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Check-out date")]
        public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);
    }
}
