// ─────────────────────────────────────────────
// Файл: Models/Category.cs
// ─────────────────────────────────────────────
using System.ComponentModel.DataAnnotations;

namespace ConstructionCompany.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть назву категорії")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        // Навігаційна властивість — список об'єктів цієї категорії
        public ICollection<RealEstateObject> RealEstateObjects { get; set; }
            = new List<RealEstateObject>();
    }
}
