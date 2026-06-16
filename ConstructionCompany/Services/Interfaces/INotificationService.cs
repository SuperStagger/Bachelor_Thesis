using ConstructionCompany.Models;

namespace ConstructionCompany.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateAsync(int userId, string message);
        Task<List<Notification>> GetByUserAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task MarkAllReadAsync(int userId);
    }
}