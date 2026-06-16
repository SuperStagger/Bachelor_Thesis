using Microsoft.EntityFrameworkCore;
using ConstructionCompany.Data;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _db;

        public NotificationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(int userId, string message)
        {
            _db.Notifications.Add(new Notification
            {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.Now
            });
            await _db.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetByUserAsync(int userId)
            => await _db.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

        public async Task<int> GetUnreadCountAsync(int userId)
            => await _db.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);

        public async Task MarkAllReadAsync(int userId)
        {
            var unread = await _db.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();
            foreach (var n in unread)
                n.IsRead = true;
            await _db.SaveChangesAsync();
        }
    }
}