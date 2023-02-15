using Entitie = Domain.Guest.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Guest.Entities;
using Domain.Guest.Ports;

namespace Data.Guests
{
    public class GuestRepository : IGuestRepository
    {
        private HotelDbContext _context;
        public GuestRepository(HotelDbContext context) 
        {
            _context = context;
        }
        public async Task<int> Create(Entitie.Guest guest)
        {
            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();
            return guest.Id;
        }

        public Task<Entitie.Guest?> Get(int Id)
        {
            return _context.Guests.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
    }
}
