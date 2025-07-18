﻿namespace RoomReservation.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();


    public Room(string name, int capacity)
        {
            Id = Guid.NewGuid();
            Name = name;
            Capacity = capacity;
        }
        public Room() { }
    }

}
