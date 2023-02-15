using Entitie = Domain.Room.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Room.Ports;

namespace Data.Room
{
    public class RoomRepository : IRoomRepository
    {
        private HotelDbContext _context;
        public RoomRepository(HotelDbContext context) 
        {
            _context = context;
        }
        public async Task<int> Create(Entitie.Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room.Id;
        }

        public Task<Entitie.Room?> Get(int Id)
        {
            return _context.Rooms.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public Task<Entitie.Room?> GetAggregate(int Id)
        {
            return _context.Rooms.Include(x => x.Bookings).Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
    }
}
