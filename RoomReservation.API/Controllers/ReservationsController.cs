using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Features.Reservations.Queries;
using RoomReservation.Domain.Enums;

namespace RoomReservation.API.Controllers;

/// <summary>
/// Controlador responsável pelas operações de reservas das salas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReservationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cria uma nova reserva.
    /// </summary>
    /// <param name="command">Dados da reserva.</param>
    /// <returns>Id da reserva criada.</returns>
    /// <response code="201">Reserva criada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Atualiza uma reserva existente.
    /// </summary>
    /// <param name="id">Id da reserva.</param>
    /// <param name="dto">Dados atualizados da reserva.</param>
    /// <response code="204">Reserva atualizada com sucesso.</response>
    /// <response code="404">Reserva não encontrada.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReservationDto dto)
    {
        var command = UpdateReservationCommand.From(id, dto);
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound("Reservation not found.");

        return NoContent();
    }

    /// <summary>
    /// Remove uma reserva pelo Id.
    /// </summary>
    /// <param name="id">Id da reserva.</param>
    /// <response code="204">Reserva removida com sucesso.</response>
    /// <response code="404">Reserva não encontrada.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteReservationCommand { Id = id });

        if (!result)
            return NotFound("Reservation not found.");

        return NoContent();
    }

    /// <summary>
    /// Retorna todas as reservas.
    /// </summary>
    /// <response code="200">Lista de reservas retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<ReservationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllReservationsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retorna uma reserva pelo Id.
    /// </summary>
    /// <param name="id">Id da reserva.</param>
    /// <response code="200">Reserva encontrada.</response>
    /// <response code="404">Reserva não encontrada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetReservationByIdQuery(id));

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Retorna as reservas com base no status informado.
    /// </summary>
    /// <param name="status">Status da reserva: Pending, Confirmed, Cancelled.</param>
    /// <response code="200">Lista de reservas retornada com sucesso.</response>
    /// <response code="400">Status inválido.</response>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(List<ReservationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByStatus(string status)
    {
        if (!Enum.TryParse<ReservationStatus>(status, true, out var parsedStatus))
        {
            return BadRequest($"Status inválido: '{status}'. Os valores válidos são: {string.Join(", ", Enum.GetNames(typeof(ReservationStatus)))}");
        }

        var query = new GetReservationsByStatusQuery(parsedStatus);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Retorna as reservas de uma sala específica.
    /// </summary>
    /// <param name="roomId">Id da sala.</param>
    /// <response code="200">Lista de reservas retornada com sucesso.</response>
    [HttpGet("room/{roomId}")]
    [ProducesResponseType(typeof(List<ReservationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRoomId(Guid roomId)
    {
        var query = new GetReservationsByRoomQuery(roomId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
