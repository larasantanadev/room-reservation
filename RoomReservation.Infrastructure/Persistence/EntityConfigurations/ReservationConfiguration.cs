using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Infrastructure.Persistence.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReservedBy)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(r => r.NumberOfAttendees)
                  .IsRequired();

            builder.Property(r => r.StartTime)
                   .IsRequired();

            builder.Property(r => r.EndTime)
                   .IsRequired();

            builder.Property(r => r.Status)
                   .IsRequired()
                   .HasConversion<string>();

            builder.HasOne(r => r.Room)
                   .WithMany()
                   .HasForeignKey(r => r.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
