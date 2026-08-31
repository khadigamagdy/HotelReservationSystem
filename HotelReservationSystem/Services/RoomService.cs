using HotelReservationSystem.Data;
using HotelReservationSystem.Enums;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly HotelDbContext _context;

        public RoomService(
            IRoomRepository roomRepository,
            HotelDbContext context)
        {
            _roomRepository = roomRepository;
            _context = context;
        }

        public List<Room> GetAll()
        {
            return _roomRepository.GetAll();
        }

        public List<Room> GetAvailableRooms()
        {
            return _roomRepository.GetAvailableRooms();
        }

        public Room? GetById(int id)
        {
            return _roomRepository.GetById(id);
        }

        public bool Create(
            Room room,
            List<int> amenityIds,
            int userId)
        {
            if (_roomRepository.RoomNumberExists(room.RoomNumber))
                return false;

            room.CreatedByUserId = userId;
            room.Status = RoomStatus.Available;

            _roomRepository.Add(room);
            _roomRepository.Save();

            foreach (var amenityId in amenityIds.Distinct())
            {
                _context.RoomAmenities.Add(new RoomAmenity
                {
                    RoomId = room.Id,
                    AmenityId = amenityId
                });
            }

            _context.SaveChanges();

            return true;
        }

        public bool Update(
            Room room,
            List<int> amenityIds,
            int userId)
        {
            var existingRoom = _roomRepository.GetById(room.Id);

            if (existingRoom == null)
                return false;

            if (_roomRepository.RoomNumberExists(
                room.RoomNumber,
                room.Id))
            {
                return false;
            }

            existingRoom.RoomNumber = room.RoomNumber;
            existingRoom.FloorNumber = room.FloorNumber;
            existingRoom.RoomTypeId = room.RoomTypeId;
            existingRoom.Status = room.Status;

            existingRoom.LastModifiedByUserId = userId;
            existingRoom.LastModifiedAt = DateTime.UtcNow;

            _context.RoomAmenities.RemoveRange(
                existingRoom.RoomAmenities);

            foreach (var amenityId in amenityIds.Distinct())
            {
                _context.RoomAmenities.Add(new RoomAmenity
                {
                    RoomId = room.Id,
                    AmenityId = amenityId
                });
            }

            _roomRepository.Update(existingRoom);
            _roomRepository.Save();

            return true;
        }

        public void Delete(int id)
        {
            var room = _roomRepository.GetById(id);

            if (room == null)
                return;

            _roomRepository.Delete(room);
            _roomRepository.Save();
        }
    }
}
