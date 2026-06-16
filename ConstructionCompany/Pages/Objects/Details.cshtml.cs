using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Objects
{
    public class DetailsModel : PageModel
    {
        private readonly IRealEstateService _realEstateService;

        public DetailsModel(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }

        public RealEstateObject Object { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var obj = await _realEstateService.GetByIdAsync(id);
            if (obj == null) return NotFound();

            Object = obj;
            return Page();
        }
    }
}
