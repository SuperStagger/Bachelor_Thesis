// Файл: Pages/Objects/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Pages.Objects
{
    public class IndexModel : PageModel
    {
        private readonly IRealEstateService _realEstateService;
        private const int PageSize = 9;

        public IndexModel(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }

        public List<RealEstateObject> Objects { get; set; } = new();
        public List<SelectListItem> Categories { get; set; } = new();

        // Пагінація
        public int CurrentPage  { get; set; } = 1;
        public int TotalPages   { get; set; } = 1;
        public int TotalCount   { get; set; } = 0;

        // Фільтри
        [BindProperty(SupportsGet = true)] public int? CategoryId     { get; set; }
        [BindProperty(SupportsGet = true)] public decimal? MinPrice    { get; set; }
        [BindProperty(SupportsGet = true)] public decimal? MaxPrice    { get; set; }
        [BindProperty(SupportsGet = true)] public decimal? MinArea     { get; set; }
        [BindProperty(SupportsGet = true)] public decimal? MaxArea     { get; set; }
        [BindProperty(SupportsGet = true)] public int? MinFloor        { get; set; }
        [BindProperty(SupportsGet = true)] public int? Rooms           { get; set; }
        [BindProperty(SupportsGet = true)] public string? BuildingStatus { get; set; }
        [BindProperty(SupportsGet = true)] public string? SaleStatus   { get; set; }
        [BindProperty(SupportsGet = true)] public int PageNumber       { get; set; } = 1;

        public async Task OnGetAsync()
        {
            CurrentPage = PageNumber < 1 ? 1 : PageNumber;

            var (items, total) = await _realEstateService.GetFilteredAsync(
                CategoryId, MinPrice, MaxPrice,
                MinArea, MaxArea, MinFloor, Rooms,
                BuildingStatus, SaleStatus,
                adminMode: false,
                page: CurrentPage, pageSize: PageSize);

            Objects    = items;
            TotalCount = total;
            TotalPages = (int)Math.Ceiling(total / (double)PageSize);

            var cats = await _realEstateService.GetCategoriesAsync();
            Categories = cats.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text  = c.Name
            }).ToList();
        }
    }
}
