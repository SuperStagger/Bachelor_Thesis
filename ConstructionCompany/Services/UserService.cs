// Файл: Services/UserService.cs
using Microsoft.EntityFrameworkCore;
using ConstructionCompany.Data;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetByEmailAsync(string email)
            => await _db.Users.FirstOrDefaultAsync(u =>
                u.Email.ToLower() == email.ToLower());

        public async Task<User?> GetByIdAsync(int id)
            => await _db.Users.FindAsync(id);

        public async Task<List<User>> GetAllAsync()
            => await _db.Users
                .Where(u => u.Role == "Customer")
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

        public async Task<bool> RegisterAsync(User user, string password)
        {
            bool exists = await _db.Users.AnyAsync(u =>
                u.Email.ToLower() == user.Email.ToLower());
            if (exists) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.Role         = "Customer";
            user.CreatedAt    = DateTime.Now;

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);
            if (user == null || user.IsBlocked) return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task ToggleBlockAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return;
            user.IsBlocked = !user.IsBlocked;
            await _db.SaveChangesAsync();
        }

        public async Task<string> ResetPasswordAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return string.Empty;

            // Генеруємо тимчасовий пароль вигляду: Temp3f8a21!
            var tempPassword = $"Temp{Guid.NewGuid().ToString("N")[..6]}!";
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword);
            await _db.SaveChangesAsync();

            return tempPassword;
        }
    }
}
