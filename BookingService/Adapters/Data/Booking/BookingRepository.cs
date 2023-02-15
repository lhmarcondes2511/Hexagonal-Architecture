using Entities = Domain.Booking.Entities;
using Domain.Booking.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Booking
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _context;

        public BookingRepository(HotelDbContext hotelDbContext)
        {
            _context = hotelDbContext;
        }

        public async Task<int> CreateBooking(Entities.Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking.Id;
        }

        public Task<Entities.Booking?> Get(int Id)
        {
            return _context.Bookings.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
    }
}
