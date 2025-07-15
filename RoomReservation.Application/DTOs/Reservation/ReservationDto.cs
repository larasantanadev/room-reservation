namespace RoomReservation.Application.DTOs.Reservation
{
    /// <summary>
    /// Representa os dados de uma reserva existente.
    /// </summary>
    public class ReservationDto
    {
        /// <summary>
        /// Identificador único da reserva.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Identificador da sala associada à reserva.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Nome da pessoa que realizou a reserva.
        /// </summary>
        public string ReservedBy { get; set; } = string.Empty;

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
        public string Status { get; set; } = string.Empty;
    }
}
