// Файл: Services/Interfaces/IRealEstateService.cs
using ConstructionCompany.Models;

namespace ConstructionCompany.Services.Interfaces
{
    public interface IRealEstateService
    {
        Task<(List<RealEstateObject> Items, int TotalCount)> GetFilteredAsync(
            int? categoryId, decimal? minPrice, decimal? maxPrice,
            decimal? minArea, decimal? maxArea,
            int? minFloor, int? rooms,
            string? buildingStatus, string? saleStatus,
            bool adminMode = false,
            int page = 1, int pageSize = 9);

        Task<List<RealEstateObject>> GetAllVisibleAsync();
        Task<List<RealEstateObject>> GetProjectsAsync();
        Task<RealEstateObject?> GetByIdAsync(int id);
        Task<List<Category>> GetCategoriesAsync();
        Task CreateAsync(RealEstateObject obj, List<IFormFile> photos);
        Task UpdateAsync(RealEstateObject obj);
        Task DeleteAsync(int id);
        Task<RealEstateObject> CopyAsync(int id);
        Task AddPhotosAsync(int objectId, List<IFormFile> photos);
        Task SetMainPhotoAsync(int photoId, int objectId);
        Task DeletePhotoAsync(int photoId);
    }
}
