using Application.Booking;
using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Requests;
using Application.Guest.Requests;
using Application.Payment.DTO;
using Application.Payment.Responses;
using Application.Responses;
using Application.Room.DTO;
using Domain.Guest.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<GuestController> _logger;
        private readonly IBookingManager _bookManager;
        public BookingController(ILogger<GuestController> logger, IBookingManager bookingManager)
        {
            _logger = logger;
            _bookManager = bookingManager;
        }

        [HttpPost]
        [Route("{bookingId}/Pay")]
        public async Task<ActionResult<PaymentResponse>> Pay(
            PaymentRequestDto paymentRequestDto, int bookingId)
        {
            paymentRequestDto.BookingId = bookingId;
            var res = await _bookManager.PayForABooking(paymentRequestDto);

            if (res.Success) return Ok(res.Data);

            return BadRequest(res);
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Post(BookingDto booking)
        {
            var request = new CreateBookingRequest
            {
                Data = booking
            };
            var res = await _bookManager.CreateBooking(request);

            if (res.Success) return Created("", res.Data);

            var errorList = Enum.GetNames(typeof(ErrorCodes)).Select(x => x.ToString()).ToArray();

            if (Array.IndexOf(errorList, res.ErrorCode.ToString()) > -1)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<BookingDto>> Get(int bookingId)
        {
            var res = await _bookManager.GetBooking(bookingId);

            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }
    }
}
