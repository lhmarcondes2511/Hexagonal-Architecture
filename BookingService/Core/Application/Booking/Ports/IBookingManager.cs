using Application.Booking.Requests;
using Application.Booking.Responses;
using Application.Payment.Responses;

namespace Application.Booking.Ports
{
    public interface IBookingManager
    {
        Task<BookingResponse> CreateBooking(CreateBookingRequest booking);
        Task<BookingResponse> GetBooking(int bookingId);
        Task<PaymentResponse> PayForABooking(PaymentRequestDto paymentRequestDto);
    }
}
