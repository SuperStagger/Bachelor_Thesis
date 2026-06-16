// Файл: Models/RealEstateObject.cs
using System.ComponentModel.DataAnnotations;

namespace ConstructionCompany.Models
{
    public class RealEstateObject
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Введіть назву об'єкту")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Введіть ціну")]
        [Range(1, double.MaxValue, ErrorMessage = "Ціна повинна бути більше 0")]
        public decimal Price { get; set; }

        public string PriceType { get; set; } = "PerObject";

        [Required(ErrorMessage = "Введіть площу")]
        [Range(1, double.MaxValue, ErrorMessage = "Площа повинна бути більше 0")]
        public decimal Area { get; set; }

        public int? Floor { get; set; }
        public int? TotalFloors { get; set; }
        public int? Rooms { get; set; }

        [Required(ErrorMessage = "Введіть адресу")]
        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;

        // Стан будівництва
        public string BuildingStatus { get; set; } = "Commissioned";

        // Статус продажу
        public string SaleStatus { get; set; } = "Available";

        // Технічний: не відображати в каталозі
        public bool IsHidden { get; set; } = false;

        // Відображати на головній сторінці
        public bool IsFeatured { get; set; } = false;
        
        // true = відображати в "Наші проекти" (завершені будівлі/комплекси)
        public bool IsProject { get; set; } = false;

        // ── Комерційна нерухомість ────────────────────────────────
        // Тип: Office / Retail / Warehouse / Industrial
        public string? CommercialType { get; set; }

        // Висота стелі (м)
        public decimal? CeilingHeight { get; set; }

        // Паркінг
        public bool HasParking { get; set; } = false;

        // Окремий вхід
        public bool HasSeparateEntrance { get; set; } = false;

        public int? ProjectYear { get; set; }
        public int? ProjectQuarter { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Category Category { get; set; } = null!;
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public Photo? MainPhoto => Photos.FirstOrDefault(p => p.IsMain)
                                   ?? Photos.FirstOrDefault();

        public string PriceDisplay => PriceType == "PerSqM"
            ? $"від {Price:N0} грн/м²"
            : $"{Price:N0} грн";

        public string BuildingStatusDisplay => BuildingStatus switch
        {
            "InProject"         => "В проекті",
            "UnderConstruction" => "Будується",
            "Commissioned"      => "Здано в експлуатацію",
            _                   => BuildingStatus
        };

        public string BuildingStatusBadgeClass => BuildingStatus switch
        {
            "InProject"         => "bg-info text-dark",
            "UnderConstruction" => "bg-primary",
            "Commissioned"      => "bg-success",
            _                   => "bg-secondary"
        };

        public string SaleStatusDisplay => SaleStatus switch
        {
            "Available" => "Доступний",
            "Reserved"  => "Зарезервований",
            "Sold"      => "Проданий",
            _           => SaleStatus
        };

        public string SaleStatusBadgeClass => SaleStatus switch
        {
            "Available" => "bg-success",
            "Reserved"  => "bg-warning text-dark",
            "Sold"      => "bg-secondary",
            _           => "bg-secondary"
        };

        public bool IsVisibleToCustomer =>
            !IsHidden && SaleStatus != "Sold";

        public bool CanBook =>
            SaleStatus == "Available" ||
            BuildingStatus is "UnderConstruction" or "InProject";

        public string? DateDisplay
        {
            get
            {
                if (ProjectYear == null) return null;
                return BuildingStatus switch
                {
                    "InProject" =>
                        $"Початок будівництва: {ProjectYear} р.",
                    "UnderConstruction" =>
                        ProjectQuarter != null
                            ? $"Здача: {ProjectQuarter} кв. {ProjectYear} р."
                            : $"Здача: {ProjectYear} р.",
                    "Commissioned" =>
                        $"Здано: {ProjectYear} р.",
                    _ => null
                };
            }
        }

        public string? CommercialTypeDisplay => CommercialType switch
        {
            "Office" => "Офіс",
            "Retail" => "Торгова площа",
            "Warehouse" => "Склад",
            "Industrial" => "Виробництво",
            _ => null
        };
    }
}
