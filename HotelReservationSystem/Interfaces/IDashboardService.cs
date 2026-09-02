using HotelReservationSystem.ViewModels;

namespace HotelReservationSystem.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
    }
}