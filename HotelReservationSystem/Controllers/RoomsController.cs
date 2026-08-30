using HotelReservationSystem.Data;
using HotelReservationSystem.Models.Entities;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    
        //[Authorize]
        public class RoomsController : Controller
        {
            private readonly IRoomService _roomService;
            private readonly HotelDbContext _context;

            public RoomsController(
                IRoomService roomService,
                HotelDbContext context)
            {
                _roomService = roomService;
                _context = context;
            }

            public IActionResult Index()
            {
                var rooms = _roomService.GetAll();

                return View(rooms);
            }

            public IActionResult Details(int id)
            {
                var room = _roomService.GetById(id);

                if (room == null)
                    return NotFound();

                return View(room);
            }

            //[Authorize(Roles = "Manager")]
            public IActionResult Create()
            {
                var vm = new RoomCreateViewModel
                {
                    RoomTypes = _context.RoomTypes.ToList(),
                    Amenities = _context.Amenities.ToList()
                };

                return View(vm);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            //[Authorize(Roles = "Manager")]
            public IActionResult Create(RoomCreateViewModel vm)
            {
                if (!ModelState.IsValid)
                {
                    vm.RoomTypes = _context.RoomTypes.ToList();
                    vm.Amenities = _context.Amenities.ToList();

                    return View(vm);
                }

                // Temporary until Member 1 finishes authentication.
                int userId = 1;

                var room = new Room
                {
                    RoomNumber = vm.RoomNumber,
                    FloorNumber = vm.FloorNumber,
                    RoomTypeId = vm.RoomTypeId
                };

                var success = _roomService.Create(
                    room,
                    vm.AmenityIds,
                    userId);

                if (!success)
                {
                    ModelState.AddModelError(
                        "RoomNumber",
                        "Room number already exists.");

                    vm.RoomTypes = _context.RoomTypes.ToList();
                    vm.Amenities = _context.Amenities.ToList();

                    return View(vm);
                }

                return RedirectToAction(nameof(Index));
            }

            //[Authorize(Roles = "Manager")]
            public IActionResult Edit(int id)
            {
                var room = _roomService.GetById(id);

                if (room == null)
                    return NotFound();

                var vm = new RoomEditViewModel
                {
                    Id = room.Id,
                    RoomNumber = room.RoomNumber,
                    FloorNumber = room.FloorNumber,
                    RoomTypeId = room.RoomTypeId,
                    Status = room.Status,
                    AmenityIds = room.RoomAmenities
                        .Select(x => x.AmenityId)
                        .ToList(),

                    RoomTypes = _context.RoomTypes.ToList(),
                    Amenities = _context.Amenities.ToList()
                };

                return View(vm);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            //[Authorize(Roles = "Manager")]
            public IActionResult Edit(RoomEditViewModel vm)
            {
                if (!ModelState.IsValid)
                {
                    vm.RoomTypes = _context.RoomTypes.ToList();
                    vm.Amenities = _context.Amenities.ToList();

                    return View(vm);
                }

                int userId = 1;

                var room = new Room
                {
                    Id = vm.Id,
                    RoomNumber = vm.RoomNumber,
                    FloorNumber = vm.FloorNumber,
                    RoomTypeId = vm.RoomTypeId,
                    Status = vm.Status
                };

                var success = _roomService.Update(
                    room,
                    vm.AmenityIds,
                    userId);

                if (!success)
                {
                    ModelState.AddModelError(
                        "RoomNumber",
                        "Room number already exists.");

                    vm.RoomTypes = _context.RoomTypes.ToList();
                    vm.Amenities = _context.Amenities.ToList();

                    return View(vm);
                }

                return RedirectToAction(nameof(Index));
            }

            [Authorize(Roles = "Manager")]
            public IActionResult Delete(int id)
            {
                var room = _roomService.GetById(id);

                if (room == null)
                    return NotFound();

                return View(room);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            //[Authorize(Roles = "Manager")]
            public IActionResult DeleteConfirmed(int id)
            {
                _roomService.Delete(id);

                return RedirectToAction(nameof(Index));
            }

        }
    
}
