// Файл: Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using ConstructionCompany.Models;

namespace ConstructionCompany.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Таблиці бази даних
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RealEstateObject> RealEstateObjects { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Users ─────────────────────────────────────────────────────
            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique(); // email унікальний
            });

            // ── Categories → RealEstateObjects (1:N, RESTRICT) ────────────
            modelBuilder.Entity<RealEstateObject>()
                .HasOne(o => o.Category)
                .WithMany(c => c.RealEstateObjects)
                .HasForeignKey(o => o.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── RealEstateObjects → Photos (1:N, CASCADE) ─────────────────
            modelBuilder.Entity<Photo>()
                .HasOne(p => p.RealEstateObject)
                .WithMany(o => o.Photos)
                .HasForeignKey(p => p.ObjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── Users → Bookings (1:N, RESTRICT) ─────────────────────────
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── RealEstateObjects → Bookings (1:N, RESTRICT) ──────────────
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.RealEstateObject)
                .WithMany(o => o.Bookings)
                .HasForeignKey(b => b.ObjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // ── Seed-дані: категорії ──────────────────────────────────────
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Квартири",
                    Description = "Квартири у нових житлових комплексах" },
                new Category { Id = 2, Name = "Будинки",
                    Description = "Приватні будинки та таунхауси" },
                new Category { Id = 3, Name = "Комерційна нерухомість",
                    Description = "Офісні та торгові приміщення" }
            );

            // ── Seed-дані: адміністратор ──────────────────────────────────
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id        = 1,
                    FirstName = "Адмін",
                    LastName  = "Системний",
                    Email     = "admin@construction.ua",
                    // Пароль: Admin123! — хешований через BCrypt
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role      = "Admin",
                    IsBlocked = false,
                    CreatedAt = new DateTime(2026, 1, 1)
                }
            );

            // ── Decimal precision ─────────────────────────────────────────
            modelBuilder.Entity<RealEstateObject>()
                .Property(o => o.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RealEstateObject>()
                .Property(o => o.Area)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RealEstateObject>()
                .Property(o => o.CeilingHeight)
                .HasPrecision(5, 2);
        }
    }
}
