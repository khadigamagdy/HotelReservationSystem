using HotelReservationSystem.Interfaces;
using HotelReservationSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = "Guest")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IGuestRepository _guestRepository;

        public ReservationController(IReservationService reservationService, IGuestRepository guestRepository)
        {
            _reservationService = reservationService;
            _guestRepository = guestRepository;
        }

        // PLACEHOLDER: assumes Member 1's AccountController stores the logged-in
        // User.Id in the standard ClaimTypes.NameIdentifier claim. Confirm this
        // against the real Login action and adjust if it uses a different claim.
        private async Task<(int UserId, int GuestId)?> GetCurrentGuestAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return null;

            var guest = await _guestRepository.GetByUserIdAsync(userId);
            if (guest == null)
                return null;

            return (userId, guest.Id);
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(SearchViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction(nameof(AvailableRooms), new { checkIn = model.CheckInDate, checkOut = model.CheckOutDate });
        }

        [HttpGet]
        public async Task<IActionResult> AvailableRooms(DateTime checkIn, DateTime checkOut)
        {
            var rooms = await _reservationService.SearchAvailableRoomsAsync(checkIn, checkOut);

            var viewModel = new AvailableRoomsViewModel
            {
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                AvailableRooms = rooms
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var room = await _reservationService.GetRoomDetailsAsync(roomId);
            if (room == null)
                return NotFound();

            var viewModel = new CreateReservationViewModel
            {
                RoomId = room.Id,
                RoomNumber = room.RoomNumber,
                RoomTypeName = room.RoomType.Name,
                BasePricePerNight = room.RoomType.BasePricePerNight,
                CheckInDate = checkIn,
                CheckOutDate = checkOut
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var current = await GetCurrentGuestAsync();
            if (current == null)
                return Forbid();

            var result = await _reservationService.CreateReservationAsync(
                current.Value.GuestId, current.Value.UserId, model.RoomId, model.CheckInDate, model.CheckOutDate);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage!);
                return View(model);
            }

            TempData["SuccessMessage"] = "Your reservation has been created.";
            return RedirectToAction(nameof(MyReservations));
        }

        [HttpGet]
        public async Task<IActionResult> MyReservations()
        {
            var current = await GetCurrentGuestAsync();
            if (current == null)
                return Forbid();

            var reservations = await _reservationService.GetMyReservationsAsync(current.Value.GuestId);
            return View(new MyReservationsViewModel { Reservations = reservations });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var current = await GetCurrentGuestAsync();
            if (current == null)
                return Forbid();

            var reservation = await _reservationService.GetReservationDetailsAsync(id, current.Value.GuestId);
            if (reservation == null)
                return NotFound();

            return View(reservation);
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(int id)
        {
            var current = await GetCurrentGuestAsync();
            if (current == null)
                return Forbid();

            var reservation = await _reservationService.GetReservationDetailsAsync(id, current.Value.GuestId);
            if (reservation == null)
                return NotFound();

            return View(reservation);
        }

        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var current = await GetCurrentGuestAsync();
            if (current == null)
                return Forbid();

            var result = await _reservationService.CancelReservationAsync(id, current.Value.GuestId);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return RedirectToAction(nameof(Details), new { id });
            }

            TempData["SuccessMessage"] = "Your reservation has been cancelled.";
            return RedirectToAction(nameof(MyReservations));
        }
    }
}
