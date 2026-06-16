// Файл: Pages/Cabinet/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Cabinet
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        private readonly INotificationService _notificationService;

        public IndexModel(IUserService userService,
                          IBookingService bookingService,
                          INotificationService notificationService)
        {
            _userService = userService;
            _bookingService = bookingService;
            _notificationService = notificationService;
        }

        public User CurrentUser { get; set; } = null!;
        public List<Booking> Bookings { get; set; } = new();
        public int UnreadCount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var user = await _userService.GetByIdAsync(userId.Value);
            if (user == null)
                return RedirectToPage("/Account/Login");

            CurrentUser = user;
            Bookings    = await _bookingService.GetByUserAsync(userId.Value);

            // Лічильник непрочитаних сповіщень
            UnreadCount = await _notificationService
                .GetUnreadCountAsync(userId.Value);

            // Зберігаємо в сесію для навбару
            HttpContext.Session.SetInt32("UnreadCount", UnreadCount);

            return Page();
        }
    }
}
