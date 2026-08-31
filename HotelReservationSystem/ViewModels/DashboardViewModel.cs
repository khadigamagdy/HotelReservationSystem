namespace HotelReservationSystem.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalRooms { get; set; }

        public int AvailableRooms { get; set; }

        public int OccupiedRooms { get; set; }

        public int MaintenanceRooms { get; set; }

        public int TotalReservations { get; set; }

        public int CheckedInGuests { get; set; }

        public int PendingPayments { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}