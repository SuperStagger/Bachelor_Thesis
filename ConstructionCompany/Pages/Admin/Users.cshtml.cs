// Файл: Pages/Admin/Users.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly IUserService _userService;

        public UsersModel(IUserService userService)
        {
            _userService = userService;
        }

        public List<User> Users { get; set; } = new();
        public string? TempPassword { get; set; }
        public int? ResetUserId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");
            Users = await _userService.GetAllAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostToggleBlockAsync(int id)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");
            await _userService.ToggleBlockAsync(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostResetPasswordAsync(int id)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");

            TempPassword = await _userService.ResetPasswordAsync(id);
            ResetUserId  = id;
            Users        = await _userService.GetAllAsync();
            return Page();
        }

        private bool IsAdmin()
            => HttpContext.Session.GetString("UserRole") == "Admin";
    }
}
