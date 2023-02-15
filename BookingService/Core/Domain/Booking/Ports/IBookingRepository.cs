namespace Domain.Booking.Ports
{
    public interface IBookingRepository
    {
        Task<Entities.Booking> Get(int bookingId);
        Task<int> CreateBooking(Entities.Booking booking);
    }
}
