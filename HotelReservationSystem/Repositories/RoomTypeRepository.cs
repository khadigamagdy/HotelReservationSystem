using HotelReservationSystem.Data;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly HotelDbContext _context;

        public RoomTypeRepository(HotelDbContext context)
        {
            _context = context;
        }

        public List<RoomType> GetAll()
        {
            return _context.RoomTypes.ToList();
        }

        public RoomType? GetById(int id)
        {
            return _context.RoomTypes.Find(id);
        }

        public void Add(RoomType roomType)
        {
            _context.RoomTypes.Add(roomType);
        }

        public void Update(RoomType roomType)
        {
            _context.RoomTypes.Update(roomType);
        }

        public void Delete(RoomType roomType)
        {
            _context.RoomTypes.Remove(roomType);
        }

        public bool Exists(int id)
        {
            return _context.RoomTypes.Any(r => r.Id == id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
