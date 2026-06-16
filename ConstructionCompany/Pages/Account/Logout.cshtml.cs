// ╔══════════════════════════════════════════════════════════════╗
// ║  Файл: Pages/Account/Logout.cshtml.cs                        ║
// ╚══════════════════════════════════════════════════════════════╝
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConstructionCompany.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Objects/Index");
        }
    }
}
