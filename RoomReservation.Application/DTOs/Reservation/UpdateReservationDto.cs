namespace RoomReservation.Application.DTOs.Reservation
{
    /// <summary>
    /// Representa os dados necessários para atualizar uma reserva existente.
    /// </summary>
    public class UpdateReservationDto
    {
        /// <summary>
        /// Identificador da sala a ser reservada.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Nome da pessoa responsável pela reserva.
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
        public RoomReservation.Domain.Enums.ReservationStatus Status { get; set; }
    }
}
