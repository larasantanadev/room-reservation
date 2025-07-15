namespace RoomReservation.Domain.Enums
{
    /// <summary>
    /// Enumeração que representa os possíveis status de uma reserva.
    /// </summary>
    public enum ReservationStatus
    {
        /// <summary>
        /// Reserva pendente de confirmação.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Reserva confirmada.
        /// </summary>
        Confirmed = 1,

        /// <summary>
        /// Reserva cancelada.
        /// </summary>
        Cancelled = 2,

        /// <summary>
        /// Reserva concluída.
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Reserva rejeitada.
        /// </summary>
        Rejected = 4
    }
}
