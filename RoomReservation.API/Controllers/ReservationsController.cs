using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Features.Reservations.Queries;

namespace RoomReservation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReservationDto dto)
    {
        var command = UpdateReservationCommand.From(id, dto);

        var result = await _mediator.Send(command);
        if (!result)
            return NotFound("Reservation not found.");

        return NoContent();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteReservationCommand { Id = id });
        if (!result)
            return NotFound("Reservation not found.");

        return NoContent();
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllReservationsQuery());
        return Ok(result);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetReservationByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

}
