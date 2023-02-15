using Entitie = Domain.Room.Entities;

namespace Domain.Room.Ports
{
    public interface IRoomRepository
    {
        Task<Entitie.Room> Get(int id);
        Task<int> Create(Entitie.Room room);
        Task<Entitie.Room?> GetAggregate(int Id);
    }
}
