using Application;
using Application.Room.Ports;
using Application.Room.Requests;
using Application.Responses;
using Application.Room.DTO;
using Domain.Room.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.Room.Responses;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class RoomController : Controller
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomManager _roomManager;

        public RoomController(ILogger<RoomController> logger, IRoomManager roomManager)
        {
            _logger = logger;
            _roomManager = roomManager;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> Post(RoomDto room)
        {
            var request = new CreateRoomRequest
            {
                Data = room
            };
            var res = await _roomManager.CreateRoom(request);

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
            var res = await _roomManager.GetRoom(roomId);

            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }
    }
}
