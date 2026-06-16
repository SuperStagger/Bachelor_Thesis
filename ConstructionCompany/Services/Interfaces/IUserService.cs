// Файл: Services/Interfaces/IUserService.cs
using ConstructionCompany.Models;

namespace ConstructionCompany.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<bool> RegisterAsync(User user, string password);
        Task<User?> LoginAsync(string email, string password);
        Task UpdateAsync(User user);
        Task ToggleBlockAsync(int id);
        Task<string> ResetPasswordAsync(int id);
    }
}
