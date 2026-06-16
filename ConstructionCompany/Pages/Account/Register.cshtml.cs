// ╔══════════════════════════════════════════════════════════════╗
// ║  Файл: Pages/Account/Register.cshtml.cs                      ║
// ╚══════════════════════════════════════════════════════════════╝
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        public string LastName { get; set; } = string.Empty;

        [BindProperty]
        public string? MiddleName { get; set; }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string? Phone { get; set; }

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            // Якщо вже авторизований — перенаправляємо
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToPage("/Cabinet/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Перевірка збігу паролів
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Паролі не збігаються";
                return Page();
            }

            var user = new User
            {
                FirstName  = FirstName,
                LastName   = LastName,
                MiddleName = MiddleName,
                Email      = Email,
                Phone      = Phone
            };

            bool success = await _userService.RegisterAsync(user, Password);

            if (!success)
            {
                ErrorMessage = "Користувач з таким email вже існує";
                return Page();
            }

            // Після реєстрації одразу авторизуємо
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserName", user.FirstName);

            return RedirectToPage("/Objects/Index");
        }
    }
}
