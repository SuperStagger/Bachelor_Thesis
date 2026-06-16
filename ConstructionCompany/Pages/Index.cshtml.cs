// Файл: Pages/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRealEstateService _realEstateService;

        public IndexModel(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }

        public List<RealEstateObject> FeaturedObjects { get; set; } = new();
        public RealEstateObject? LatestObject { get; set; }
        public int TotalObjects { get; set; }

        public async Task OnGetAsync()
        {
            // Беремо всі видимі для статистики і featured
            var all = await _realEstateService.GetAllVisibleAsync();

            TotalObjects    = all.Count;
            FeaturedObjects = all.Where(o => o.IsFeatured).Take(6).ToList();
            LatestObject    = all.OrderByDescending(o => o.CreatedAt).FirstOrDefault();
        }
    }
}
