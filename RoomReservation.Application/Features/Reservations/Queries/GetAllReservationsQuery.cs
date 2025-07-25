﻿using MediatR;
using RoomReservation.Application.DTOs.Reservation;

namespace RoomReservation.Application.Features.Reservations.Queries
{
    public class GetAllReservationsQuery : IRequest<List<ReservationDto>>
    {
    }
}
