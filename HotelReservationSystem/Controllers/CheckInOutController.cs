using HotelReservationSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = "Receptionist,Manager")]
    public class CheckInOutController : Controller
    {
        private readonly ICheckInOutService _checkInOutService;

        public CheckInOutController(ICheckInOutService checkInOutService)
        {
            _checkInOutService = checkInOutService;
        }

        // =========================
        // Reservations
        // =========================

        [HttpGet]
        public async Task<IActionResult> Reservations()
        {
            var checkInReservations =
                await _checkInOutService.GetReservationsForCheckInAsync();

            var checkOutReservations =
                await _checkInOutService.GetReservationsForCheckOutAsync();

            ViewBag.CheckInReservations = checkInReservations;
            ViewBag.CheckOutReservations = checkOutReservations;

            return View();
        }

        // =========================
        // Check-In - GET
        // =========================

        [HttpGet]
        public async Task<IActionResult> CheckIn(int id)
        {
            var reservations =
                await _checkInOutService.GetReservationsForCheckInAsync();

            var reservation = reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                TempData["ErrorMessage"] =
                    "The reservation cannot be checked in.";

                return RedirectToAction(nameof(Reservations));
            }

            return View(reservation);
        }

        // =========================
        // Check-In - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckIn(int id)
        {
            var userIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Forbid();
            }

            var success =
                await _checkInOutService.CheckInAsync(id, userId);

            if (!success)
            {
                TempData["ErrorMessage"] =
                    "Check-in failed. Please verify the reservation and room status.";

                return RedirectToAction(nameof(Reservations));
            }

            TempData["SuccessMessage"] =
                "Guest checked in successfully.";

            return RedirectToAction(nameof(Reservations));
        }

        // =========================
        // Check-Out - GET
        // =========================

        [HttpGet]
        public async Task<IActionResult> CheckOut(int id)
        {
            var reservations =
                await _checkInOutService.GetReservationsForCheckOutAsync();

            var reservation = reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                TempData["ErrorMessage"] =
                    "The reservation cannot be checked out.";

                return RedirectToAction(nameof(Reservations));
            }

            var totalPaid = reservation.Payments.Sum(p => p.Amount);

            ViewBag.TotalPaid = totalPaid;

            ViewBag.RemainingAmount =
                Math.Max(0, reservation.TotalPrice - totalPaid);

            return View(reservation);
        }

        // =========================
        // Check-Out - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckOut(int id)
        {
            var userIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Forbid();
            }

            var result =
                await _checkInOutService.CheckOutAsync(id, userId);

            if (!result.Success)
            {
                TempData["ErrorMessage"] =
                    result.ErrorMessage;

                return RedirectToAction(nameof(Reservations));
            }

            return RedirectToAction(
                nameof(CheckoutConfirmation),
                new { id });
        }

        // =========================
        // Checkout Confirmation
        // =========================

        [HttpGet]
        public IActionResult CheckoutConfirmation(int id)
        {
            ViewBag.ReservationId = id;

            return View();
        }
    }
}