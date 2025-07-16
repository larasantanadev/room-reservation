using RoomReservation.Domain.Enums;

namespace RoomReservation.Application.DTOs.Reservation
{
    /// <summary>
    /// Objeto utilizado para criar uma nova reserva.
    /// </summary>
    public class CreateReservationDto
    {
        /// <summary>
        /// Identificador da sala reservada.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Nome da pessoa que está realizando a reserva.
        /// </summary>
        public string ReservedBy { get; set; } = string.Empty;

        /// <summary>
        /// Número de pessoas previstas para a reserva.
        /// </summary>
        public int NumberOfAttendees { get; set; }

        /// <summary>
        /// Data e hora de início da reserva.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Data e hora de término da reserva.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Status atual da reserva.
        /// </summary>
        public ReservationStatus Status { get; set; }
    }
}
