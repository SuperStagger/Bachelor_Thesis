// Файл: Pages/Cabinet/Notifications.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Cabinet
{
    public class NotificationsModel : PageModel
    {
        private readonly INotificationService _notificationService;

        public NotificationsModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public List<Notification> Notifications { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login");

            // Завантажуємо всі сповіщення
            Notifications = await _notificationService
                .GetByUserAsync(userId.Value);

            // Позначаємо як прочитані
            await _notificationService.MarkAllReadAsync(userId.Value);

            // Обнуляємо лічильник у сесії
            HttpContext.Session.SetInt32("UnreadCount", 0);

            return Page();
        }
    }
}
