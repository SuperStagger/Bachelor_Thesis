// Файл: Pages/Admin/Objects.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Admin
{
    public class ObjectsModel : PageModel
    {
        private readonly IRealEstateService _realEstateService;
        private const int PageSize = 15;

        public ObjectsModel(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }

        public List<RealEstateObject> Objects   { get; set; } = new();
        public List<SelectListItem>  Categories { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages  { get; set; } = 1;
        public int TotalCount  { get; set; } = 0;

        [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = 1;

        [BindProperty]
        public RealEstateObject EditObject { get; set; } = new();

        [BindProperty]
        public List<IFormFile> Photos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");

            CurrentPage = PageNumber < 1 ? 1 : PageNumber;

            var (items, total) = await _realEstateService.GetFilteredAsync(
                null, null, null, null, null, null, null,
                null, null, adminMode: true,
                page: CurrentPage, pageSize: PageSize);

            Objects    = items;
            TotalCount = total;
            TotalPages = (int)Math.Ceiling(total / (double)PageSize);

            await LoadCategories();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");
            await _realEstateService.CreateAsync(EditObject, Photos);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCopyAsync(int id)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");
            var copy = await _realEstateService.CopyAsync(id);
            return RedirectToPage("/Admin/EditObject", new { id = copy.Id });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");
            await _realEstateService.DeleteAsync(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSetMainPhotoAsync(
            int photoId, int objectId)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");
            await _realEstateService.SetMainPhotoAsync(photoId, objectId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeletePhotoAsync(
            int photoId, int objectId)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");
            await _realEstateService.DeletePhotoAsync(photoId);
            return RedirectToPage();
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
