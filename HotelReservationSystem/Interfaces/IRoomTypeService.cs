using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Interfaces
{
    public interface IRoomTypeService
    {
        List<RoomType> GetAll();
        RoomType? GetById(int id);
        void Add(RoomType roomType);
        void Update(RoomType roomType);
        void Delete(int id);
    }
}
