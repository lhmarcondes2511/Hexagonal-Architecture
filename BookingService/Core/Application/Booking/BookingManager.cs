using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Requests;
using Application.Booking.Responses;
using Application.Payment.Ports;
using Application.Payment.Responses;
using Application.Responses;
using Domain.Booking.DomainExceptions;
using Domain.Booking.Ports;
using Domain.Guest.Ports;
using Domain.Room.Ports;

namespace Application.Booking
{
    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;

        public BookingManager(IBookingRepository bookingRepository, IGuestRepository guestRepository, IRoomRepository roomRepository, IPaymentProcessorFactory paymentProcessorFactory)
        {
            _bookingRepository = bookingRepository;
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
            _paymentProcessorFactory = paymentProcessorFactory;
        }

        public async Task<BookingResponse> CreateBooking(BookingDto request)
        {
            try
            {
                var booking = BookingDto.MapToEntity(request);
                booking.Guest = await _guestRepository.Get(request.GuestId);
                booking.Room = await _roomRepository.GetAggregate(request.RoomId);

                await booking.Save(_bookingRepository);

                request.Id = booking.Id;

                return new BookingResponse
                {
                    Data = request,
                    Success = true,
                };
            }
            catch(PlacedAtIsARequiredInformationException e)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch(RoomCannotBeBookingException e)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.RROM_CANNOT_BOOKING,
                    Message = "Missing required information passed"
                };
            }
            catch(Exception)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
        }

        public async Task<PaymentResponse> PayForABooking(PaymentRequestDto paymentRequestDto)
        {
            var paymentProcessor = _paymentProcessorFactory.GetPaymentProcessor(paymentRequestDto.SelectedPaymentProvider);

            var response = await paymentProcessor.CapturePayment(paymentRequestDto.PaymentIntention);

            if(response.Success)
            {
                return new PaymentResponse
                {
                    Data = response.Data,
                    Success = true,
                    Message = "Payment successfuly processed"
                };
            }

            return response;
        }

        public async Task<BookingResponse> GetBooking(int bookingId)
        {
            var booking = await _bookingRepository.Get(bookingId);

            if (booking == null)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_NOT_FOUND,
                    Message = "No Guest record has found with the given ID"
                };
            }

            return new BookingResponse
            {
                Data = BookingDto.MapToDto(booking),
                Success = true,
            };
        }
    }
}
