using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Admin
{
    public class PhotosModel : PageModel
    {
        private readonly IRealEstateService _realEstateService;

        public PhotosModel(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }

        public Models.RealEstateObject Object { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");

            var obj = await _realEstateService.GetByIdAsync(id);
            if (obj == null) return NotFound();

            Object = obj;
            return Page();
        }

        public async Task<IActionResult> OnPostSetMainAsync(int photoId, int objectId)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");
            await _realEstateService.SetMainPhotoAsync(photoId, objectId);
            return RedirectToPage(new { id = objectId });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int photoId, int objectId)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");
            await _realEstateService.DeletePhotoAsync(photoId);
            return RedirectToPage(new { id = objectId });
        }

        public async Task<IActionResult> OnPostAddAsync(
            int objectId, List<IFormFile> photos)
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");
            await _realEstateService.AddPhotosAsync(objectId, photos);
            return RedirectToPage(new { id = objectId });
        }

        private bool IsAdmin()
            => HttpContext.Session.GetString("UserRole") == "Admin";
    }
}