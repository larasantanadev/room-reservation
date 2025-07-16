using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoomReservation.Application.DTOs.Room;
using RoomReservation.Application.Features.Rooms.Commands;
using RoomReservation.Application.Features.Rooms.Queries;

namespace RoomReservation.API.Controllers;

/// <summary>
/// Controlador responsável pelas operações de salas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RoomController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cria uma nova sala.
    /// </summary>
    /// <param name="command">Dados da sala a ser criada.</param>
    /// <returns>Retorna o ID da nova sala criada.</returns>
    /// <response code="201">Sala criada com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRoomCommand command)
    {
        var roomId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id = roomId }, roomId);
    }

    /// <summary>
    /// Retorna todas as salas cadastradas.
    /// </summary>
    /// <returns>Lista de salas.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<RoomDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RoomDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllRoomsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retorna os dados de uma sala pelo ID.
    /// </summary>
    /// <param name="id">Identificador da sala.</param>
    /// <returns>Dados da sala correspondente.</returns>
    /// <response code="200">Sala encontrada com sucesso.</response>
    /// <response code="404">Sala não encontrada.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetRoomByIdQuery(id));
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Lista salas disponíveis entre o intervalo de tempo informado.
    /// </summary>
    /// <param name="startTime">Data e hora de início.</param>
    /// <param name="endTime">Data e hora de fim.</param>
    /// <returns>Salas disponíveis no intervalo especificado.</returns>
    /// <response code="200">Lista de salas disponíveis retornada com sucesso.</response>
    [HttpGet("available")]
    [ProducesResponseType(typeof(List<RoomDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        var query = new GetAvailableRoomsQuery(startTime, endTime);
        var rooms = await _mediator.Send(query);
        return Ok(rooms);
    }
}
