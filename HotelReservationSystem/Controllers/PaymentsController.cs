using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models;
using HotelReservationSystem.Models.Entities;
using HotelReservationSystem.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = "Receptionist,Manager")]
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: Payments/Create/5
        [HttpGet]
        public async Task<IActionResult> Create(int reservationId)
        {
            var payments = await _paymentService
                .GetPaymentsByReservationAsync(reservationId);

            ViewBag.ReservationId = reservationId;
            ViewBag.TotalPaid = payments.Sum(p => p.Amount);

            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int reservationId,
            decimal amount,
            PaymentMethod method)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError(
                    "amount",
                    "Payment amount must be greater than zero.");

                var payments = await _paymentService
                    .GetPaymentsByReservationAsync(reservationId);

                ViewBag.ReservationId = reservationId;
                ViewBag.TotalPaid = payments.Sum(p => p.Amount);

                return View();
            }

            var userId = GetCurrentUserId();

            var result = await _paymentService.AddPaymentAsync(
                reservationId,
                amount,
                method,
                userId);

            if (!result)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to record the payment.");

                return View();
            }

            return RedirectToAction(
                nameof(ReservationPayments),
                new { reservationId });
        }

        // GET: Payments/ReservationPayments/5
        [HttpGet]
        public async Task<IActionResult> ReservationPayments(int reservationId)
        {
            var payments = await _paymentService
                .GetPaymentsByReservationAsync(reservationId);

            ViewBag.ReservationId = reservationId;
            ViewBag.TotalPaid = payments.Sum(p => p.Amount);

            return View(payments);
        }

        // GET: Payments/PaymentDetails/5
        [HttpGet]
        public async Task<IActionResult> PaymentDetails(int id)
        {
            var payment = await _paymentService
                .GetPaymentByIdAsync(id);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;

            if (int.TryParse(userIdClaim.Value, out var userId))
                return userId;

            return null;
        }
    }
}



