using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Interfaces
{
    public interface IRoomService
    {
        List<Room> GetAll();
        List<Room> GetAvailableRooms();
        Room? GetById(int id);

        bool Create(
            Room room,
            List<int> amenityIds,
            int userId);

        bool Update(
            Room room,
            List<int> amenityIds,
            int userId);

        void Delete(int id);
    }
}
