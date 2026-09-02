using HotelReservationSystem.Models;

namespace HotelReservationSystem.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);

        Task<IEnumerable<Payment>> GetByReservationIdAsync(int reservationId);

        Task<Reservation?> GetReservationByIdAsync(int reservationId);

        Task AddAsync(Payment payment);

        Task<decimal> GetTotalPaidAsync(int reservationId);

        Task SaveChangesAsync();
    }
}