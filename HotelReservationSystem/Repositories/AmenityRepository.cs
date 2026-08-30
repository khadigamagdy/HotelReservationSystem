using HotelReservationSystem.Data;
using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Repositories
{
    public class AmenityRepository : IAmenityRepository
    {
        private readonly HotelDbContext _context;

        public AmenityRepository(HotelDbContext context)
        {
            _context = context;
        }

        public List<Amenity> GetAll()
        {
            return _context.Amenities.ToList();
        }

        public Amenity? GetById(int id)
        {
            return _context.Amenities.Find(id);
        }

        public void Add(Amenity amenity)
        {
            _context.Amenities.Add(amenity);
        }

        public void Update(Amenity amenity)
        {
            _context.Amenities.Update(amenity);
        }

        public void Delete(Amenity amenity)
        {
            _context.Amenities.Remove(amenity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
