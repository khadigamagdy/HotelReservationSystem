using HotelReservationSystem.Data;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models;
using HotelReservationSystem.Models.Entities;
using HotelReservationSystem.Enums;
using HotelReservationSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly HotelDbContext context;

        public DashboardService(HotelDbContext context)
        {
            this.context = context;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var dashboard = new DashboardViewModel
            {
                TotalRooms = await context.Rooms.CountAsync(),

                AvailableRooms = await context.Rooms.CountAsync(
                    room => room.Status == RoomStatus.Available),

                OccupiedRooms = await context.Rooms.CountAsync(
                    room => room.Status == RoomStatus.Occupied),

                MaintenanceRooms = await context.Rooms.CountAsync(
                    room => room.Status == RoomStatus.Maintenance),

                TotalReservations =
                    await context.Reservations.CountAsync(),

                CheckedInGuests =
                    await context.Reservations.CountAsync(
                        reservation =>
                            reservation.BookingStatus ==
                            BookingStatus.CheckedIn),

                PendingPayments =
                    await context.Reservations.CountAsync(
                        reservation =>
                            reservation.PaymentStatus !=
                            PaymentStatus.Paid),

                TotalRevenue = await context.Payments
                    .SumAsync(payment =>
                        (decimal?)payment.Amount) ?? 0
            };

            return dashboard;
        }
    }
}

