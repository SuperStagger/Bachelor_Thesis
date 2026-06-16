// ╔══════════════════════════════════════════════════════════════╗
// ║  Файл: Pages/Admin/Bookings.cshtml.cs                        ║
// ╚══════════════════════════════════════════════════════════════╝
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Admin
{
    public class BookingsModel : PageModel
    {
        private readonly IBookingService _bookingService;

        public BookingsModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public List<Booking> Bookings { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");

            Bookings = await _bookingService.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(
            int id, string status)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");

            await _bookingService.UpdateStatusAsync(id, status);
            return RedirectToPage();
        }

        private bool IsAdmin()
            => HttpContext.Session.GetString("UserRole") == "Admin";
    }
}
