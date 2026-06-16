// ─────────────────────────────────────────────
// Файл: Models/Photo.cs
// ─────────────────────────────────────────────
using System.ComponentModel.DataAnnotations;

namespace ConstructionCompany.Models
{
    public class Photo
    {
        public int Id { get; set; }

        // Зовнішній ключ до таблиці RealEstateObjects
        public int ObjectId { get; set; }

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;  // шлях: /uploads/guid.jpg

        [MaxLength(200)]
        public string? AltText { get; set; }

        public bool IsMain { get; set; } = false;

        // Навігаційна властивість
        public RealEstateObject RealEstateObject { get; set; } = null!;
    }
}
