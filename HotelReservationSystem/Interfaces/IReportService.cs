using HotelReservationSystem.Models;
using HotelReservationSystem.ViewModels;

namespace HotelReservationSystem.Interfaces
{
    public interface IReportService
    {
        Task<List<Reservation>> GetReservationsAsync();

        Task<List<Payment>> GetPaymentsAsync();

        Task<List<Room>> GetRoomsAsync();

        Task<List<RevenueReportViewModel>>
            GetRevenueAsync();
    }
}