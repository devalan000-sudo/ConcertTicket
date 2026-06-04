using ConcertTicket.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConcertTicket.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}

        //DbSets representan nuestras tablas
        public DbSet<User> Users {  get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Conversion de Enums a String
            modelBuilder.Entity<User>().
                Property(u => u.Role).
                HasConversion<string>();

            modelBuilder.Entity<Event>().
                Property(e => e.Category).
                HasConversion<string>();

            modelBuilder.Entity<Reservation>().
                Property(r => r.Status).
                HasConversion<string>();

            // 2. Configuracion de precision para decimales (Precios)
            modelBuilder.Entity<Zone>().
                Property(z => z.Price).
                HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Reservation>().
                Property(r => r.TotalAmount).
                HasColumnType("decimal(18,2)");

            //3.Relaciones explicitas

            //Un Evento tiene muchas Zonas
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Zones)
                .WithOne(z => z.Event)
                .HasForeignKey(z => z.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            //Una Reserva tiene muchos Tickets
            modelBuilder.Entity<Reservation>()
                .HasMany(r => r.Tickets)
                .WithOne(t => t.Reservation)
                .HasForeignKey(t => t.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
