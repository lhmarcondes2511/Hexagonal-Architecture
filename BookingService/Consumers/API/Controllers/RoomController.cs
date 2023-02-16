using Application;
using Application.Room.Ports;
using Application.Room.Requests;
using Application.Responses;
using Application.Room.DTO;
using Domain.Room.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.Room.Responses;
using MediatR;
using Application.Booking.Commands;
using Application.Room.Commands;
using Application.Room.Queries;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class RoomController : Controller
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomManager _roomManager;
        private readonly IMediator _mediator;

        public RoomController(ILogger<RoomController> logger, IRoomManager roomManager, IMediator mediator)
        {
            _logger = logger;
            _roomManager = roomManager;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> Post(RoomDto room)
        {
            var command = new CreateRoomCommand
            {
                RoomDto = room
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
        public async Task<ActionResult<RoomDto>> Get(int roomId)
        {
            var query = new GetRoomQuery
            {
                RoomId = roomId
            };

            var res = await _mediator.Send(query);

            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }
    }
}
