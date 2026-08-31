using System.Globalization;
using HotelReservationSystem.Data;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models;
using HotelReservationSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly HotelDbContext context;

        public ReportService(HotelDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Reservation>>
            GetReservationsAsync()
        {
            return await context.Reservations
                .Include(reservation => reservation.Guest)
                    .ThenInclude(guest => guest.User)
                .Include(reservation => reservation.Room)
                    .ThenInclude(room => room.RoomType)
                .Include(reservation => reservation.Payments)
                .OrderByDescending(
                    reservation => reservation.CheckInDate)
                .ToListAsync();
        }

        public async Task<List<Payment>>
            GetPaymentsAsync()
        {
            return await context.Payments
                .Include(payment => payment.Reservation)
                    .ThenInclude(reservation =>
                        reservation.Guest)
                    .ThenInclude(guest => guest.User)
                .Include(payment => payment.RecordedByUser)
                .OrderByDescending(payment => payment.PaidAt)
                .ToListAsync();
        }

        public async Task<List<Room>>
            GetRoomsAsync()
        {
            return await context.Rooms
                .Include(room => room.RoomType)
                .Include(room => room.CreatedByUser)
                .Include(room => room.LastModifiedByUser)
                .OrderBy(room => room.RoomNumber)
                .ToListAsync();
        }

        public async Task<List<RevenueReportViewModel>>
            GetRevenueAsync()
        {
            var payments = await context.Payments
                .OrderBy(payment => payment.PaidAt)
                .ToListAsync();

            return payments
                .GroupBy(payment => new
                {
                    payment.PaidAt.Year,
                    payment.PaidAt.Month
                })
                .Select(group =>
                    new RevenueReportViewModel
                    {
                        Year = group.Key.Year,
                        Month = group.Key.Month,

                        MonthName =
                            CultureInfo.CurrentCulture
                                .DateTimeFormat
                                .GetMonthName(group.Key.Month),

                        PaymentsCount = group.Count(),

                        TotalRevenue =
                            group.Sum(payment => payment.Amount)
                    })
                .OrderByDescending(report => report.Year)
                .ThenByDescending(report => report.Month)
                .ToList();
        }
    }
}