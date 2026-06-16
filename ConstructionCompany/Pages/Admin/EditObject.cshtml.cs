// Файл: Pages/Admin/EditObject.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Admin
{
    public class EditObjectModel : PageModel
    {
        private readonly IRealEstateService _realEstateService;

        public EditObjectModel(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }

        public List<SelectListItem> Categories { get; set; } = new();

        [BindProperty]
        public RealEstateObject EditObject { get; set; } = new();

        [BindProperty]
        public List<IFormFile> Photos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");

            var obj = await _realEstateService.GetByIdAsync(id);
            if (obj == null) return NotFound();

            EditObject = obj;
            await LoadCategories();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");

            var existing = await _realEstateService.GetByIdAsync(EditObject.Id);
            if (existing == null) return NotFound();

            existing.Title          = EditObject.Title;
            existing.CategoryId     = EditObject.CategoryId;
            existing.Price          = EditObject.Price;
            existing.PriceType      = EditObject.PriceType;
            existing.Area           = EditObject.Area;
            existing.Floor          = EditObject.Floor;
            existing.TotalFloors    = EditObject.TotalFloors;
            existing.Rooms          = EditObject.Rooms;
            existing.Address        = EditObject.Address;
            existing.BuildingStatus = EditObject.BuildingStatus;
            existing.SaleStatus     = EditObject.SaleStatus;
            existing.IsHidden              = EditObject.IsHidden;
            existing.IsFeatured            = EditObject.IsFeatured;
            existing.IsProject             = EditObject.IsProject;
            existing.CommercialType        = EditObject.CommercialType;
            existing.CeilingHeight         = EditObject.CeilingHeight;
            existing.HasParking            = EditObject.HasParking;
            existing.HasSeparateEntrance   = EditObject.HasSeparateEntrance;
            existing.ProjectYear    = EditObject.ProjectYear;
            existing.ProjectQuarter = EditObject.ProjectQuarter;
            existing.Description    = EditObject.Description;

            await _realEstateService.UpdateAsync(existing);

            if (Photos != null && Photos.Any(p => p.Length > 0))
                await _realEstateService.AddPhotosAsync(existing.Id, Photos);

            return RedirectToPage("/Admin/Objects");
        }

        private async Task LoadCategories()
        {
            var cats = await _realEstateService.GetCategoriesAsync();
            Categories = cats.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text  = c.Name
            }).ToList();
        }

        private bool IsAdmin()
            => HttpContext.Session.GetString("UserRole") == "Admin";
    }
}
