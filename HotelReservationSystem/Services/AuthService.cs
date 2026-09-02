using HotelReservationSystem.Enums;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models;
using HotelReservationSystem.Models.Entities;
using HotelReservationSystem.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HotelReservationSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            IUserRepository userRepository,
            IGuestRepository guestRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _guestRepository = guestRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResult> RegisterGuestAsync(RegisterViewModel model)
        {
            if (await _userRepository.EmailExistsAsync(model.Email))
                return AuthResult.Fail("This email is already registered.");

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Role = UserRole.Guest,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var guest = new Guest
            {
                UserId = user.Id,
                NationalIdOrPassport = model.NationalIdOrPassport,
                Phone = model.Phone
            };

            await _guestRepository.AddAsync(guest);
            await _guestRepository.SaveChangesAsync();

            return AuthResult.Ok(user);
        }

        public async Task<AuthResult> RegisterReceptionistAsync(CreateStaffViewModel model, int createdByUserId)
        {
            if (await _userRepository.EmailExistsAsync(model.Email))
                return AuthResult.Fail("This email is already registered.");

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Role = UserRole.Receptionist,
                CreatedByUserId = createdByUserId,   // ties it back to the Manager who created it
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return AuthResult.Ok(user);
        }


        public async Task<User?> ValidateCredentialsAsync(LoginViewModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            return result == PasswordVerificationResult.Success
                || result == PasswordVerificationResult.SuccessRehashNeeded
                ? user
                : null;
        }
    }
}
