using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models;
using HotelReservationSystem.Models.Entities;
using HotelReservationSystem.Enums;

namespace HotelReservationSystem.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByReservationAsync(int reservationId)
        {
            return await _paymentRepository.GetByReservationIdAsync(reservationId);
        }

        public async Task<bool> AddPaymentAsync(
            int reservationId,
            decimal amount,
            PaymentMethod method,
            int? recordedByUserId)
        {
            if (amount <= 0)
                return false;

            var reservation = await _paymentRepository
                .GetReservationByIdAsync(reservationId);

            if (reservation == null)
                return false;

            var totalPaid = await _paymentRepository
                .GetTotalPaidAsync(reservationId);

            var remainingAmount = reservation.TotalPrice - totalPaid;

            if (amount > remainingAmount)
                return false;

            var payment = new Payment
            {
                ReservationId = reservationId,
                Amount = amount,
                Method = method,
                RecordedByUserId = recordedByUserId,
                PaidAt = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            var newTotalPaid = totalPaid + amount;

            if (newTotalPaid <= 0)
            {
                reservation.PaymentStatus = PaymentStatus.Unpaid;
            }
            else if (newTotalPaid < reservation.TotalPrice)
            {
                reservation.PaymentStatus = PaymentStatus.PartiallyPaid;
            }
            else
            {
                reservation.PaymentStatus = PaymentStatus.Paid;
            }

            await _paymentRepository.SaveChangesAsync();

            return true;
        }

        public async Task<PaymentStatus> GetPaymentStatusAsync(int reservationId)
        {
            var reservation = await _paymentRepository
                .GetReservationByIdAsync(reservationId);

            if (reservation == null)
                return PaymentStatus.Unpaid;

            var totalPaid = await _paymentRepository
                .GetTotalPaidAsync(reservationId);

            if (totalPaid <= 0)
                return PaymentStatus.Unpaid;

            if (totalPaid < reservation.TotalPrice)
                return PaymentStatus.PartiallyPaid;

            return PaymentStatus.Paid;
        }

        public async Task<decimal> GetTotalPaidAsync(int reservationId)
        {
            return await _paymentRepository
                .GetTotalPaidAsync(reservationId);
        }
    }
}

