// Файл: Pages/Projects/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Projects
{
    public class IndexModel : PageModel
    {
        private readonly IRealEstateService _realEstateService;

        public IndexModel(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }

        public List<RealEstateObject> Projects { get; set; } = new();

        public async Task OnGetAsync()
        {
            Projects = await _realEstateService.GetProjectsAsync();
        }
    }
}
