// Файл: Pages/Cabinet/Edit.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Cabinet
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;

        public EditModel(IUserService userService)
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
        public string? Phone { get; set; }

        [BindProperty]
        public string? CurrentPassword { get; set; }

        [BindProperty]
        public string? NewPassword { get; set; }

        [BindProperty]
        public string? ConfirmNewPassword { get; set; }

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var user = await _userService.GetByIdAsync(userId.Value);
            if (user == null)
                return RedirectToPage("/Account/Login");

            // Заповнюємо форму поточними даними
            FirstName  = user.FirstName;
            LastName   = user.LastName;
            MiddleName = user.MiddleName;
            Phone      = user.Phone;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var user = await _userService.GetByIdAsync(userId.Value);
            if (user == null)
                return RedirectToPage("/Account/Login");

            // Оновлюємо основні дані
            user.FirstName  = FirstName;
            user.LastName   = LastName;
            user.MiddleName = MiddleName;
            user.Phone      = Phone;

            // Зміна пароля — тільки якщо заповнено
            if (!string.IsNullOrEmpty(NewPassword))
            {
                if (NewPassword != ConfirmNewPassword)
                {
                    ErrorMessage = "Нові паролі не збігаються";
                    return Page();
                }

                if (!BCrypt.Net.BCrypt.Verify(CurrentPassword, user.PasswordHash))
                {
                    ErrorMessage = "Поточний пароль введено невірно";
                    return Page();
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            }

            await _userService.UpdateAsync(user);

            // Оновлюємо ім'я в сесії
            HttpContext.Session.SetString("UserName", user.FirstName);

            SuccessMessage = "Профіль успішно оновлено";
            return Page();
        }
    }
}
