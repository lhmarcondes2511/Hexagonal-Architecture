using Entities = Domain.Booking.Entities;
using Domain.Booking.Ports;
using Microsoft.EntityFrameworkCore;
using Domain.Room.Entities;

namespace Data.Booking
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _context;

        public BookingRepository(HotelDbContext hotelDbContext)
        {
            _context = hotelDbContext;
        }

        public async Task<Entities.Booking> CreateBooking(Entities.Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public Task<Entities.Booking?> Get(int Id)
        {
            return _context.Bookings.Include(x => x.Guest).Include(x => x.Room).Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
    }
}
