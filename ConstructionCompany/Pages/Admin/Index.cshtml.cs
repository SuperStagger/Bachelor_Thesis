// Файл: Pages/Admin/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IRealEstateService _realEstateService;
        private readonly IBookingService    _bookingService;
        private readonly IUserService       _userService;

        public IndexModel(IRealEstateService realEstateService,
                          IBookingService    bookingService,
                          IUserService       userService)
        {
            _realEstateService = realEstateService;
            _bookingService    = bookingService;
            _userService       = userService;
        }

        public int ObjectsCount  { get; set; }
        public int BookingsCount { get; set; }
        public int UsersCount    { get; set; }
        public int NewBookings   { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");

            // Для статистики беремо всі без пагінації
            var (objects, total) = await _realEstateService.GetFilteredAsync(
                null, null, null, null, null, null, null,
                null, null, adminMode: true,
                page: 1, pageSize: int.MaxValue);

            var bookings = await _bookingService.GetAllAsync();
            var users    = await _userService.GetAllAsync();

            ObjectsCount  = total;
            BookingsCount = bookings.Count;
            UsersCount    = users.Count;
            NewBookings   = bookings.Count(b => b.BookingStatus == "New");

            return Page();
        }

        private bool IsAdmin()
            => HttpContext.Session.GetString("UserRole") == "Admin";
    }
}
