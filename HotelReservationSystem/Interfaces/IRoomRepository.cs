using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Interfaces
{
    public interface IRoomRepository
    {

        List<Room> GetAll();
        List<Room> GetAvailableRooms();
        Room? GetById(int id);
        void Add(Room room);
        void Update(Room room);
        void Delete(Room room);
        bool RoomNumberExists(string roomNumber, int? excludeId = null);
        void Save();
    }
}

