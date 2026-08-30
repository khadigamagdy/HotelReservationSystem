using HotelReservationSystem.Data;
using HotelReservationSystem.Enums;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext _context;

        public RoomRepository(HotelDbContext context)
        {
            _context = context;
        }

        public List<Room> GetAll()
        {
            return _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.Amenity)
                .ToList();
        }

        public List<Room> GetAvailableRooms()
        {
            return _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.Amenity)
                .Where(r => r.Status == RoomStatus.Available)
                .ToList();
        }

        public Room? GetById(int id)
        {
            return _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.Amenity)
                .FirstOrDefault(r => r.Id == id);
        }

        public void Add(Room room)
        {
            _context.Rooms.Add(room);
        }

        public void Update(Room room)
        {
            _context.Rooms.Update(room);
        }

        public void Delete(Room room)
        {
            _context.Rooms.Remove(room);
        }

        public bool RoomNumberExists(string roomNumber, int? excludeId = null)
        {
            return _context.Rooms.Any(r =>
                r.RoomNumber == roomNumber &&
                (!excludeId.HasValue || r.Id != excludeId.Value));
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
