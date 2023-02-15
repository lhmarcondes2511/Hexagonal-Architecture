using Application.Guest.DTO;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class GuestController : Controller
    {
        private readonly ILogger<GuestController> _logger;
        private readonly IGuestManager _guestManager;

        public GuestController(ILogger<GuestController> logger, IGuestManager guestManager)
        {
            _logger = logger;
            _guestManager = guestManager;
        }

        [HttpPost]
        public async Task<ActionResult<GuestDto>> Post(GuestDto guest)
        {
            var request = new CreateGuestRequest
            {
                Data = guest
            };
            var res = await _guestManager.CreateGuest(request);

            if (res.Success) return Created("", res.Data);

            var errorList = Enum.GetNames(typeof(ErrorCodes)).Select(x => x.ToString()).ToArray();

            if(Array.IndexOf(errorList, res.ErrorCode.ToString()) > -1)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<GuestDto>> Get(int guestId)
        {
            var res = await _guestManager.GetGuest(guestId);
            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }
    }
}
