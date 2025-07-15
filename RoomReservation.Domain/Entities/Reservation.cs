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
    }
}
