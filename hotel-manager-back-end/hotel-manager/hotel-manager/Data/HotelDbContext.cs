using hotel_manager.Entities;
using Microsoft.EntityFrameworkCore;

namespace hotel_manager.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(
            DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }

        public DbSet<Hotel> Hotels => Set<Hotel>();

        public DbSet<Client> Clients => Set<Client>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var client = modelBuilder.Entity<Client>();

            client.ToTable("Clients", "dbo");

            client.HasKey(c => c.Id);

            client.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            client.Property(c => c.Name)
                .HasMaxLength(150)
                .IsRequired();

            client.Property(c => c.Email)
                .HasMaxLength(254)
                .IsRequired();

            client.Property(c => c.PasswordHash)
                .IsRequired();

            var hotel = modelBuilder.Entity<Hotel>();

            hotel.ToTable("Hotels", "dbo");

            hotel.HasKey(h => h.Id);

            hotel.Property(h => h.Id)
                .ValueGeneratedOnAdd();

            hotel.Property(h => h.Cnpj)
                .HasMaxLength(20)
                .IsRequired();

            hotel.Property(h => h.Name)
                .HasMaxLength(150)
                .IsRequired();

            hotel.Property(h => h.Localization)
                .HasMaxLength(300)
                .IsRequired();

            hotel.Property(h => h.HotelType)
                .HasMaxLength(50)
                .IsRequired();

            hotel.HasOne(h => h.Client)
                .WithMany(c => c.Hotels)
                .HasForeignKey(h => h.ClientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}