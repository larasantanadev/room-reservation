namespace RoomReservation.Application.DTOs.Room
{
    public class CreateRoomDto
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }
}
