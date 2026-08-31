using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly HotelDbContext _context;

        public GuestRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<Guest?> GetByIdAsync(int id) =>
            await _context.Guests.Include(g => g.User)
                                  .FirstOrDefaultAsync(g => g.Id == id);

        public async Task<Guest?> GetByUserIdAsync(int userId) =>
            await _context.Guests.Include(g => g.User)
                                  .FirstOrDefaultAsync(g => g.UserId == userId);

        public async Task AddAsync(Guest guest) =>
            await _context.Guests.AddAsync(guest);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }

}
