using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Interfaces
{
    public interface IRoomTypeRepository
    {
        List<RoomType> GetAll();
        RoomType? GetById(int id);
        void Add(RoomType roomType);
        void Update(RoomType roomType);
        void Delete(RoomType roomType);
        bool Exists(int id);
        void Save();

    }
}
