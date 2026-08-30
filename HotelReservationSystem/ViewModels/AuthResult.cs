using HotelReservationSystem.Models;

namespace HotelReservationSystem.ViewModels
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public User? User { get; set; }

        public static AuthResult Fail(string message) => new() { Success = false, ErrorMessage = message };
        public static AuthResult Ok(User user) => new() { Success = true, User = user };
    }
}
