using HotelReservationSystem.Data;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models;
using HotelReservationSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly HotelDbContext _context;

        public PaymentRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Reservation)
                .Include(p => p.RecordedByUser)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Payment>> GetByReservationIdAsync(int reservationId)
        {
            return await _context.Payments
                .Include(p => p.Reservation)
                .Include(p => p.RecordedByUser)
                .Where(p => p.ReservationId == reservationId)
                .OrderByDescending(p => p.PaidAt)
                .ToListAsync();
        }

        public async Task<Reservation?> GetReservationByIdAsync(int reservationId)
        {
            return await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == reservationId);
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }

        public async Task<decimal> GetTotalPaidAsync(int reservationId)
        {
            return await _context.Payments
                .Where(p => p.ReservationId == reservationId)
                .SumAsync(p => p.Amount);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
