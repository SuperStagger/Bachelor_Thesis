// ─────────────────────────────────────────────
// Файл: Models/Booking.cs
// ─────────────────────────────────────────────
using System.ComponentModel.DataAnnotations;

namespace ConstructionCompany.Models
{
    public class Booking
    {
        public int Id { get; set; }

        // Зовнішні ключі
        public int UserId { get; set; }
        public int ObjectId { get; set; }

        // "Purchase" або "Consultation"
        [Required]
        public string BookingType { get; set; } = "Consultation";

        // "New", "InProgress", "Completed", "Cancelled"
        public string BookingStatus { get; set; } = "New";

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime? PreferredDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Навігаційні властивості
        public User User { get; set; } = null!;
        public RealEstateObject RealEstateObject { get; set; } = null!;

        // Зручні властивості для відображення статусів
        public string BookingTypeDisplay => BookingType switch
        {
            "Purchase"     => "Придбання",
            "Consultation" => "Консультація",
            _              => BookingType
        };

        public string BookingStatusDisplay => BookingStatus switch
        {
            "New"        => "Нова",
            "InProgress" => "В обробці",
            "Completed"  => "Завершена",
            "Cancelled"  => "Скасована",
            _            => BookingStatus
        };
    }
}
