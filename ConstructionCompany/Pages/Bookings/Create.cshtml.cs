// Файл: Pages/Bookings/Create.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IRealEstateService _realEstateService;

        public CreateModel(IBookingService bookingService,
                           IRealEstateService realEstateService)
        {
            _bookingService = bookingService;
            _realEstateService = realEstateService;
        }

        public RealEstateObject Object { get; set; } = null!;

        [BindProperty]
        public string BookingType { get; set; } = "Consultation";

        [BindProperty]
        public string? Comment { get; set; }

        [BindProperty]
        public string? PreferredDate { get; set; }

        [BindProperty]
        public string? PreferredDateStr { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Перевірка авторизації
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var obj = await _realEstateService.GetByIdAsync(id);
            if (obj == null) return NotFound();

            // Не можна подати заявку на недоступний об'єкт
            if (!obj.CanBook)
            {
                ErrorMessage = "Цей об'єкт недоступний для заявок";
                return RedirectToPage("/Objects/Details", new { id });
            }

            Object = obj;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login");

            DateTime? preferredDate = null;
            if (!string.IsNullOrEmpty(PreferredDateStr) &&
               DateTime.TryParseExact(PreferredDateStr, "dd.MM.yyyy",
               System.Globalization.CultureInfo.InvariantCulture,
               System.Globalization.DateTimeStyles.None, out var parsed))
               {
                  preferredDate = parsed;
               }

               var booking = new Booking
               {
                  UserId        = userId.Value,
                  ObjectId      = id,
                  BookingType   = BookingType,
                  Comment       = Comment,
                  PreferredDate = preferredDate
               };

            await _bookingService.CreateAsync(booking);

            // Після подачі заявки — в кабінет
            return RedirectToPage("/Cabinet/Index");
        }
    }
}
