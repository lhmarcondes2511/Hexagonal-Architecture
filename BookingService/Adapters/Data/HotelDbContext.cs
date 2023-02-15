using Data.Guests;
using Data.Room;
using Microsoft.EntityFrameworkCore;
using Domain.Guest.Entities;

namespace Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Domain.Room.Entities.Room> Rooms { get; set; }
        public virtual DbSet<Domain.Booking.Entities.Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GuestConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
        }
    }
}