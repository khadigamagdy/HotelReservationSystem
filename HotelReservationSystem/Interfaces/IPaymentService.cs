
using HotelReservationSystem.Models.Entities;
using HotelReservationSystem.Enums;

namespace HotelReservationSystem.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment?> GetPaymentByIdAsync(int id);

        Task<IEnumerable<Payment>> GetPaymentsByReservationAsync(int reservationId);

        Task<bool> AddPaymentAsync(
            int reservationId,
            decimal amount,
            PaymentMethod method,
            int? recordedByUserId);

        Task<PaymentStatus> GetPaymentStatusAsync(int reservationId);

        Task<decimal> GetTotalPaidAsync(int reservationId);
    }
}



