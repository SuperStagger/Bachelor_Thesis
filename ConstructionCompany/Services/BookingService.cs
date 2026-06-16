// Файл: Services/BookingService.cs
using Microsoft.EntityFrameworkCore;
using ConstructionCompany.Data;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _db;

        public BookingService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Booking>> GetByUserAsync(int userId)
            => await _db.Bookings
                .Include(b => b.RealEstateObject)
                    .ThenInclude(o => o.Photos)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

        public async Task<List<Booking>> GetAllAsync()
            => await _db.Bookings
                .Include(b => b.User)
                .Include(b => b.RealEstateObject)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

        public async Task<Booking?> GetByIdAsync(int id)
            => await _db.Bookings
                .Include(b => b.User)
                .Include(b => b.RealEstateObject)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task CreateAsync(Booking booking)
        {
            booking.CreatedAt     = DateTime.Now;
            booking.BookingStatus = "New";
            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var booking = await _db.Bookings
                .Include(b => b.RealEstateObject)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return;

            booking.BookingStatus = status;
            booking.UpdatedAt     = DateTime.Now;
            await _db.SaveChangesAsync();

            // Не сповіщаємо про статус "New" — він початковий
            if (status == "New") return;

            var statusText = status switch
            {
                "InProgress" => "взята в обробку",
                "Completed"  => "завершена",
                "Cancelled"  => "скасована",
                _            => status
            };

            var message = $"Статус вашої заявки на " +
                          $"«{booking.RealEstateObject.Title}» " +
                          $"змінено: {statusText}.";

            _db.Notifications.Add(new Notification
            {
                UserId    = booking.UserId,
                Message   = message,
                CreatedAt = DateTime.Now
            });
            await _db.SaveChangesAsync();
        }
    }
}
