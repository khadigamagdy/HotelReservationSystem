using HotelReservationSystem.Enums;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models;
using HotelReservationSystem.Models.Entities;
using HotelReservationSystem.ViewModels;

namespace HotelReservationSystem.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<IEnumerable<Room>> SearchAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
        {
            return await _reservationRepository.GetAvailableRoomsAsync(checkIn, checkOut);
        }

        public async Task<Room?> GetRoomDetailsAsync(int roomId)
        {
            return await _reservationRepository.GetRoomByIdAsync(roomId);
        }

        public async Task<ReservationResult> CreateReservationAsync(int guestId, int userId, int roomId, DateTime checkIn, DateTime checkOut)
        {
            if (checkIn.Date < DateTime.UtcNow.Date)
                return ReservationResult.Fail("Check-in date cannot be in the past.");

            if (checkOut.Date <= checkIn.Date)
                return ReservationResult.Fail("Check-out date must be after check-in date.");

            bool isAvailable = await _reservationRepository.IsRoomAvailableAsync(roomId, checkIn, checkOut);
            if (!isAvailable)
                return ReservationResult.Fail("This room is no longer available for the selected dates.");

            var room = await _reservationRepository.GetRoomByIdAsync(roomId);
            if (room == null)
                return ReservationResult.Fail("Room not found.");

            int nights = (checkOut.Date - checkIn.Date).Days;
            decimal totalPrice = nights * room.RoomType.BasePricePerNight;

            var reservation = new Reservation
            {
                GuestId = guestId,
                RoomId = roomId,
                CreatedByUserId = userId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                TotalPrice = totalPrice,
                PaymentStatus = PaymentStatus.Unpaid,
                BookingStatus = BookingStatus.Booked
            };

            await _reservationRepository.AddAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            return ReservationResult.Ok(reservation);
        }

        public async Task<IEnumerable<Reservation>> GetMyReservationsAsync(int guestId)
        {
            return await _reservationRepository.GetByGuestIdAsync(guestId);
        }

        public async Task<Reservation?> GetReservationDetailsAsync(int reservationId, int guestId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);

            if (reservation == null || reservation.GuestId != guestId)
                return null;

            return reservation;
        }

        public async Task<ReservationResult> CancelReservationAsync(int reservationId, int guestId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);

            if (reservation == null)
                return ReservationResult.Fail("Reservation not found.");

            if (reservation.GuestId != guestId)
                return ReservationResult.Fail("You are not allowed to cancel this reservation.");

            if (reservation.BookingStatus == BookingStatus.CheckedIn ||
                reservation.BookingStatus == BookingStatus.CheckedOut)
                return ReservationResult.Fail("This reservation cannot be cancelled after check-in.");

            reservation.BookingStatus = BookingStatus.Cancelled;
            await _reservationRepository.SaveChangesAsync();

            return ReservationResult.Ok(reservation);
        }
    }
}