using Entitie = Domain.Guest.Entities;

namespace Domain.Guest.Ports
{
    public interface IGuestRepository
    {
        Task<Entitie.Guest> Get(int id);
        Task<int> Create(Entitie.Guest guest);
    }
}
