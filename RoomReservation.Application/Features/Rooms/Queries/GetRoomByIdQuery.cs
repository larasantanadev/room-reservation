using MediatR;
using RoomReservation.Application.DTOs.Room;

namespace RoomReservation.Application.Features.Rooms.Queries;

public record GetRoomByIdQuery(Guid Id) : IRequest<RoomDto>;
