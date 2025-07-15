using System;

namespace RoomReservation.Application.DTOs.Room
{
    /// <summary>
    /// Representa os dados de uma sala cadastrada.
    /// </summary>
    public class RoomDto
    {
        /// <summary>
        /// Identificador único da sala.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nome da sala.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Capacidade máxima da sala.
        /// </summary>
        public int Capacity { get; set; }
    }
}
