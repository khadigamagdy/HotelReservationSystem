using HotelReservationSystem.Interfaces;
using HotelReservationSystem.Models.Entities;

namespace HotelReservationSystem.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _repository;

        public RoomTypeService(IRoomTypeRepository repository)
        {
            _repository = repository;
        }

        public List<RoomType> GetAll()
        {
            return _repository.GetAll();
        }

        public RoomType? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(RoomType roomType)
        {
            _repository.Add(roomType);
            _repository.Save();
        }

        public void Update(RoomType roomType)
        {
            _repository.Update(roomType);
            _repository.Save();
        }

        public void Delete(int id)
        {
            var roomType = _repository.GetById(id);

            if (roomType == null)
                return;

            _repository.Delete(roomType);
            _repository.Save();
        }
    }
}
