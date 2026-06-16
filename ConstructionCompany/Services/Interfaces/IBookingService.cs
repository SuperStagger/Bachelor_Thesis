// ╔══════════════════════════════════════════════════════════════╗
// ║  Файл: Services/Interfaces/IBookingService.cs                ║
// ╚══════════════════════════════════════════════════════════════╝
using ConstructionCompany.Models;

namespace ConstructionCompany.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<Booking>> GetByUserAsync(int userId);
        Task<List<Booking>> GetAllAsync();           // для адміна
        Task<Booking?> GetByIdAsync(int id);
        Task CreateAsync(Booking booking);
        Task UpdateStatusAsync(int id, string status);
    }
}