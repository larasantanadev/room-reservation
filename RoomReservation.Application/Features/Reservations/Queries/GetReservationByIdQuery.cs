using MediatR;
using RoomReservation.Application.DTOs.Reservation;

namespace RoomReservation.Application.Features.Reservations.Queries;

public record GetReservationByIdQuery(Guid Id) : IRequest<ReservationDto>;
