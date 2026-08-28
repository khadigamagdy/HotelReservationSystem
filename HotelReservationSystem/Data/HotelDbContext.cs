using HotelReservationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationSystem.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<RoomAmenity> RoomAmenities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            modelBuilder.Entity<Room>()
                .HasIndex(room => room.RoomNumber)
                .IsUnique();

            modelBuilder.Entity<Guest>()
                .HasIndex(guest => guest.UserId)
                .IsUnique();

            modelBuilder.Entity<Amenity>()
                .HasIndex(amenity => amenity.Name)
                .IsUnique();

            modelBuilder.Entity<RoomAmenity>()
                .HasKey(roomAmenity => new
                {
                    roomAmenity.RoomId,
                    roomAmenity.AmenityId
                });

            modelBuilder.Entity<User>()
                .HasOne(user => user.CreatedByUser)
                .WithMany(user => user.CreatedUsers)
                .HasForeignKey(user => user.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Guest>()
                .HasOne(guest => guest.User)
                .WithOne(user => user.Guest)
                .HasForeignKey<Guest>(guest => guest.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>()
                .HasOne(room => room.RoomType)
                .WithMany(roomType => roomType.Rooms)
                .HasForeignKey(room => room.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                .HasOne(room => room.CreatedByUser)
                .WithMany(user => user.CreatedRooms)
                .HasForeignKey(room => room.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Room>()
                .HasOne(room => room.LastModifiedByUser)
                .WithMany(user => user.ModifiedRooms)
                .HasForeignKey(room => room.LastModifiedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RoomAmenity>()
                .HasOne(roomAmenity => roomAmenity.Room)
                .WithMany(room => room.RoomAmenities)
                .HasForeignKey(roomAmenity => roomAmenity.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoomAmenity>()
                .HasOne(roomAmenity => roomAmenity.Amenity)
                .WithMany(amenity => amenity.RoomAmenities)
                .HasForeignKey(roomAmenity => roomAmenity.AmenityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(reservation => reservation.Guest)
                .WithMany(guest => guest.Reservations)
                .HasForeignKey(reservation => reservation.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(reservation => reservation.Room)
                .WithMany(room => room.Reservations)
                .HasForeignKey(reservation => reservation.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(reservation => reservation.CreatedByUser)
                .WithMany(user => user.CreatedReservations)
                .HasForeignKey(reservation => reservation.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reservation>()
                .HasOne(reservation => reservation.CheckedInByUser)
                .WithMany(user => user.CheckedInReservations)
                .HasForeignKey(reservation => reservation.CheckedInByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reservation>()
                .HasOne(reservation => reservation.CheckedOutByUser)
                .WithMany(user => user.CheckedOutReservations)
                .HasForeignKey(reservation => reservation.CheckedOutByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Payment>()
                .HasOne(payment => payment.Reservation)
                .WithMany(reservation => reservation.Payments)
                .HasForeignKey(payment => payment.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(payment => payment.RecordedByUser)
                .WithMany(user => user.RecordedPayments)
                .HasForeignKey(payment => payment.RecordedByUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}