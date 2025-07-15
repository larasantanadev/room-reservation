namespace RoomReservation.Application.DTOs.Room
{
    /// <summary>
    /// Representa os dados necessários para criar uma nova sala.
    /// </summary>
    public class CreateRoomDto
    {
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
