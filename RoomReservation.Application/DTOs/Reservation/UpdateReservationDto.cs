namespace RoomReservation.Application.DTOs.Reservation
{
    public class UpdateReservationDto
    {
        public Guid RoomId { get; set; }
        public string ReservedBy { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public RoomReservation.Domain.Enums.ReservationStatus Status { get; set; }
    }
}
