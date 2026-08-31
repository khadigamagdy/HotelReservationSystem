using HotelReservationSystem.Data;
using HotelReservationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Controllers
{
    [Authorize(Roles = "Manager")]
    public class UserManagementController : Controller
    {
        private readonly HotelDbContext context;

        public UserManagementController(
            HotelDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await context.Users
                .Include(user => user.Guest)
                .OrderBy(user => user.FullName)
                .ToListAsync();

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(
            int id,
            UserRole role)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Role = role;

            await context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"{user.FullName}'s role was updated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}