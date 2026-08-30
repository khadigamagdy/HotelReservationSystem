using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{
    //[Authorize]
    public class RoomTypesController : Controller
    {
        private readonly IRoomTypeService _service;

        public RoomTypesController(IRoomTypeService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var roomTypes = _service.GetAll();
            return View(roomTypes);
        }

        //[Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Manager")]
        public IActionResult Create(RoomType roomType)
        {
            if (!ModelState.IsValid)
                return View(roomType);

            _service.Add(roomType);

            return RedirectToAction(nameof(Index));
        }

        //[Authorize(Roles = "Manager")]
        public IActionResult Edit(int id)
        {
            var roomType = _service.GetById(id);

            if (roomType == null)
                return NotFound();

            return View(roomType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Manager")]
        public IActionResult Edit(RoomType roomType)
        {
            if (!ModelState.IsValid)
                return View(roomType);

            _service.Update(roomType);

            return RedirectToAction(nameof(Index));
        }

        //[Authorize(Roles = "Manager")]
        public IActionResult Delete(int id)
        {
            var roomType = _service.GetById(id);

            if (roomType == null)
                return NotFound();

            return View(roomType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Manager")]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
