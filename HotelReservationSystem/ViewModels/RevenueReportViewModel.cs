namespace HotelReservationSystem.ViewModels
{
    public class RevenueReportViewModel
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public string MonthName { get; set; } = string.Empty;

        public int PaymentsCount { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}