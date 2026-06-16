// ╔══════════════════════════════════════════════════════════════╗
// ║  Файл: Pages/Account/Login.cshtml.cs                         ║
// ╚══════════════════════════════════════════════════════════════╝
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToPage("/Cabinet/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.LoginAsync(Email, Password);

            if (user == null)
            {
                ErrorMessage = "Невірний email або пароль, або акаунт заблоковано";
                return Page();
            }

            // Зберігаємо дані у сесію
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserName", user.FirstName);

            // Адмін іде в адмін-панель, покупець — в кабінет
            if (user.Role == "Admin")
                return RedirectToPage("/Admin/Index");

            return RedirectToPage("/Cabinet/Index");
        }
    }
}
