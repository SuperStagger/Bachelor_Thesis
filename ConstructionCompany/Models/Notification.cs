using System.ComponentModel.DataAnnotations;

namespace ConstructionCompany.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Навігаційна властивість
        public User User { get; set; } = null!;
    }
}