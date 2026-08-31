using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.Controllers
{

    //[Authorize]
    public class AmenitiesController : Controller
    {
        private readonly IAmenityRepository _repository;

        public AmenitiesController(IAmenityRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.GetAll());
        }

        //[Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Manager")]
        public IActionResult Create(Amenity amenity)
        {
            if (!ModelState.IsValid)
                return View(amenity);

            _repository.Add(amenity);
            _repository.Save();

            return RedirectToAction(nameof(Index));
        }

        //[Authorize(Roles = "Manager")]
        public IActionResult Edit(int id)
        {
            var amenity = _repository.GetById(id);

            if (amenity == null)
                return NotFound();

            return View(amenity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Manager")]
        public IActionResult Edit(Amenity amenity)
        {
            if (!ModelState.IsValid)
                return View(amenity);

            _repository.Update(amenity);
            _repository.Save();

            return RedirectToAction(nameof(Index));
        }

        //[Authorize(Roles = "Manager")]
        public IActionResult Delete(int id)
        {
            var amenity = _repository.GetById(id);

            if (amenity == null)
                return NotFound();

            return View(amenity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Manager")]
        public IActionResult DeleteConfirmed(int id)
        {
            var amenity = _repository.GetById(id);

            if (amenity != null)
            {
                _repository.Delete(amenity);
                _repository.Save();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
