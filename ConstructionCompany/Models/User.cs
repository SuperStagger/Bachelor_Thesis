// ─────────────────────────────────────────────
// Файл: Models/User.cs
// ─────────────────────────────────────────────
using System.ComponentModel.DataAnnotations;

namespace ConstructionCompany.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть ім'я")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть прізвище")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Введіть email")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        // Пароль зберігається тільки у хешованому вигляді (BCrypt)
        public string PasswordHash { get; set; } = string.Empty;

        // "Customer" або "Admin"
        public string Role { get; set; } = "Customer";

        public bool IsBlocked { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Навігаційна властивість — список заявок цього користувача
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        // Зручна властивість для відображення повного імені
        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }
}

