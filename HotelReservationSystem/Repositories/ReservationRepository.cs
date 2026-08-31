using HotelReservationSystem.Data;
using HotelReservationSystem.Enums;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models;
using HotelReservationSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly HotelDbContext _context;

        public ReservationRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation?> GetByIdAsync(int id) =>
            await _context.Reservations
                .Include(r => r.Guest)
                    .ThenInclude(g => g.User)
                .Include(r => r.Room)
                    .ThenInclude(room => room.RoomType)
                .Include(r => r.Payments)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<IEnumerable<Reservation>> GetByGuestIdAsync(int guestId) =>
            await _context.Reservations
                .Include(r => r.Room)
                    .ThenInclude(room => room.RoomType)
                .Where(r => r.GuestId == guestId)
                .OrderByDescending(r => r.CheckInDate)
                .ToListAsync();

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut) =>
            await _context.Rooms
                .Include(room => room.RoomType)
                .Where(room => room.Status != RoomStatus.Maintenance)
                .Where(room => !_context.Reservations.Any(r =>
                    r.RoomId == room.Id &&
                    r.BookingStatus != BookingStatus.Cancelled &&
                    r.CheckInDate < checkOut &&
                    r.CheckOutDate > checkIn))
                .ToListAsync();

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null || room.Status == RoomStatus.Maintenance)
                return false;

            bool hasConflict = await _context.Reservations.AnyAsync(r =>
                r.RoomId == roomId &&
                r.BookingStatus != BookingStatus.Cancelled &&
                r.CheckInDate < checkOut &&
                r.CheckOutDate > checkIn);

            return !hasConflict;
        }

        public async Task AddAsync(Reservation reservation) =>
            await _context.Reservations.AddAsync(reservation);

        public async Task<Room?> GetRoomByIdAsync(int roomId) =>
            await _context.Rooms
            .Include(room => room.RoomType)
            .FirstOrDefaultAsync(room => room.Id == roomId);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
