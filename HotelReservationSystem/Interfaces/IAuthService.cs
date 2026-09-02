using HotelReservationSystem.Models;
using HotelReservationSystem.ViewModels;
using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterGuestAsync(RegisterViewModel model);
        Task<User?> ValidateCredentialsAsync(LoginViewModel model);
        Task<AuthResult> RegisterReceptionistAsync(CreateStaffViewModel model, int createdByUserId);
    }
}
