using HotelReservationSystem.Models;
using HotelReservationSystem.ViewModels;

namespace HotelReservationSystem.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterGuestAsync(RegisterViewModel model);
        Task<User?> ValidateCredentialsAsync(LoginViewModel model);
    }
}
