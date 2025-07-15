using RoomReservation.Domain.Enums;

namespace RoomReservation.Application.DTOs.Reservation
{
    public class CreateReservationDto
    {
        public Guid RoomId { get; set; }
        public string ReservedBy { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ReservationStatus Status { get; set; }
    }
}
