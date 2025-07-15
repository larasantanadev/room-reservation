using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoomReservation.Application.DTOs.Room;
using RoomReservation.Application.Features.Rooms.Commands;
using RoomReservation.Application.Features.Rooms.Queries;

namespace RoomReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateRoomCommand command)
        {
            var roomId = await _mediator.Send(command);
            return CreatedAtAction(nameof(Create), new { id = roomId }, roomId);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<RoomDto>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllRoomsQuery());
            return Ok(result);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            var query = new GetAvailableRoomsQuery(startTime, endTime);
            var rooms = await _mediator.Send(query);
            return Ok(rooms);
        }

    }
}
