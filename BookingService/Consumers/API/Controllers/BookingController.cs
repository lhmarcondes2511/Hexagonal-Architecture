using Application.Booking;
using Application.Booking.Commands;
using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Queries;
using Application.Booking.Requests;
using Application.Guest.Requests;
using Application.Payment.DTO;
using Application.Payment.Responses;
using Application.Responses;
using Application.Room.DTO;
using Domain.Guest.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<GuestController> _logger;
        private readonly IBookingManager _bookManager;
        private readonly IMediator _mediator;
        public BookingController(ILogger<GuestController> logger, IBookingManager bookingManager, IMediator mediator)
        {
            _logger = logger;
            _bookManager = bookingManager;
            _mediator = mediator;
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
            var command = new CreateBookingCommand
            {
                BookingDto = booking,
            };

            var res = await _mediator.Send(command);

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
            var query = new GetBookingQuery
            {
                Id = bookingId
            };

            var res = await _mediator.Send(query);

            if (res.Success) return Created("", res.Data);

            _logger.LogError("Could not process the request", res);
            return BadRequest(res);
        }
    }
}
