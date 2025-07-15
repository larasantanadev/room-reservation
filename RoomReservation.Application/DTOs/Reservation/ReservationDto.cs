namespace RoomReservation.Application.DTOs.Reservation
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string ReservedBy { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
