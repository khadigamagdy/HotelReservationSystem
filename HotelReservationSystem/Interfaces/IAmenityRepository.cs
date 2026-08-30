using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Interfaces
{
    public interface IAmenityRepository
    {
        List<Amenity> GetAll();
        Amenity? GetById(int id);
        void Add(Amenity amenity);
        void Update(Amenity amenity);
        void Delete(Amenity amenity);
        void Save();
    }
}
