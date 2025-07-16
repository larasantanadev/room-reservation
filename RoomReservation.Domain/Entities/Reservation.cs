using RoomReservation.Domain.Enums;

namespace RoomReservation.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string ReservedBy { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;

        public Room? Room { get; set; }

        public Reservation(Guid roomId, string reservedBy, DateTime startTime, DateTime endTime, ReservationStatus status = ReservationStatus.Confirmed)
        {
            Id = Guid.NewGuid();
            RoomId = roomId;
            ReservedBy = reservedBy;
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
        }
        public Reservation() { }
    }
}
