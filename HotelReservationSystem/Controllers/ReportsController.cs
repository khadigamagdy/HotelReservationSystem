using HotelReservationSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ReportsController : Controller
    {
        private readonly IReportService reportService;

        public ReportsController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        public async Task<IActionResult> Reservations()
        {
            var reservations =
                await reportService.GetReservationsAsync();

            return View(reservations);
        }

        public async Task<IActionResult> Payments()
        {
            var payments =
                await reportService.GetPaymentsAsync();

            return View(payments);
        }

        public async Task<IActionResult> Rooms()
        {
            var rooms =
                await reportService.GetRoomsAsync();

            return View(rooms);
        }

        public async Task<IActionResult> Revenue()
        {
            var revenue =
                await reportService.GetRevenueAsync();

            return View(revenue);
        }
    }
}