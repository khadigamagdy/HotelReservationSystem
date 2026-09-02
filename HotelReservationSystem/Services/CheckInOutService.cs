using HotelReservationSystem.Data;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models.Entities;
using HotelReservationSystem.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Services
{
    public class CheckInOutService : ICheckInOutService
    {
        private readonly HotelDbContext _context;

        public CheckInOutService(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsForCheckInAsync()
        {
            return await _context.Reservations
                .Include(r => r.Guest)
                    .ThenInclude(g => g.User)
                .Include(r => r.Room)
                    .ThenInclude(room => room.RoomType)
                .Where(r =>
                    r.BookingStatus == BookingStatus.Booked &&
                    r.CheckInDate.Date <= DateTime.UtcNow.Date &&
                    r.CheckOutDate.Date > DateTime.UtcNow.Date)
                .OrderBy(r => r.CheckInDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsForCheckOutAsync()
        {
            return await _context.Reservations
                .Include(r => r.Guest)
                    .ThenInclude(g => g.User)
                .Include(r => r.Room)
                    .ThenInclude(room => room.RoomType)
                .Include(r => r.Payments)
                .Where(r =>
                    r.BookingStatus == BookingStatus.CheckedIn)
                .OrderBy(r => r.CheckOutDate)
                .ToListAsync();
        }

        public async Task<bool> CheckInAsync(int reservationId, int userId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
                return false;

            if (reservation.BookingStatus != BookingStatus.Booked)
                return false;

            if (reservation.Room.Status != RoomStatus.Available)
                return false;

            reservation.BookingStatus = BookingStatus.CheckedIn;
            reservation.CheckedInByUserId = userId;

            reservation.Room.Status = RoomStatus.Occupied;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<(bool Success, string? ErrorMessage)> CheckOutAsync(
            int reservationId,
            int userId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
            {
                return (false, "Reservation not found.");
            }

            if (reservation.BookingStatus != BookingStatus.CheckedIn)
            {
                return (false, "Only checked-in reservations can be checked out.");
            }

            var totalPaid = reservation.Payments.Sum(p => p.Amount);

            if (totalPaid < reservation.TotalPrice)
            {
                return (false,
                    $"Payment is incomplete. Remaining amount: {reservation.TotalPrice - totalPaid} EGP.");
            }

            reservation.BookingStatus = BookingStatus.CheckedOut;
            reservation.CheckedOutByUserId = userId;

            reservation.Room.Status = RoomStatus.Available;

            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}


