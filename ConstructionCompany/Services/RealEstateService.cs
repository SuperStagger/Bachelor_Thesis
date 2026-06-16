// Файл: Services/RealEstateService.cs
using Microsoft.EntityFrameworkCore;
using ConstructionCompany.Data;
using ConstructionCompany.Models;
using ConstructionCompany.Services.Interfaces;

namespace ConstructionCompany.Services
{
    public class RealEstateService : IRealEstateService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public RealEstateService(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db  = db;
            _env = env;
        }

        public async Task<(List<RealEstateObject> Items, int TotalCount)> GetFilteredAsync(
            int? categoryId, decimal? minPrice, decimal? maxPrice,
            decimal? minArea, decimal? maxArea,
            int? minFloor, int? rooms,
            string? buildingStatus, string? saleStatus,
            bool adminMode = false,
            int page = 1, int pageSize = 9)
        {
            var query = _db.RealEstateObjects
                .Include(o => o.Photos)
                .Include(o => o.Category)
                .AsQueryable();

            if (!adminMode)
            {
                query = query.Where(o => !o.IsHidden);
                query = query.Where(o => o.SaleStatus != "Sold");
            }

            if (categoryId.HasValue)
                query = query.Where(o => o.CategoryId == categoryId);
            if (minPrice.HasValue)
                query = query.Where(o => o.Price >= minPrice);
            if (maxPrice.HasValue)
                query = query.Where(o => o.Price <= maxPrice);
            if (minArea.HasValue)
                query = query.Where(o => o.Area >= minArea);
            if (maxArea.HasValue)
                query = query.Where(o => o.Area <= maxArea);
            if (minFloor.HasValue)
                query = query.Where(o => o.Floor >= minFloor);
            if (rooms.HasValue)
            {
                // 5 у фільтрі трактується як "5 і більше кімнат"
                if (rooms.Value >= 5)
                    query = query.Where(o => o.Rooms >= 5);
                else
                    query = query.Where(o => o.Rooms == rooms);
            }
            if (!string.IsNullOrEmpty(buildingStatus))
                query = query.Where(o => o.BuildingStatus == buildingStatus);
            if (!string.IsNullOrEmpty(saleStatus))
                query = query.Where(o => o.SaleStatus == saleStatus);

            query = query.OrderByDescending(o => o.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // Всі видимі об'єкти без пагінації (для головної)
        public async Task<List<RealEstateObject>> GetAllVisibleAsync()
            => await _db.RealEstateObjects
                .Include(o => o.Photos)
                .Include(o => o.Category)
                .Where(o => !o.IsHidden && o.SaleStatus != "Sold")
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

        // Завершені проекти — тільки ті що адмін позначив IsProject = true
        public async Task<List<RealEstateObject>> GetProjectsAsync()
            => await _db.RealEstateObjects
                .Include(o => o.Photos)
                .Include(o => o.Category)
                .Where(o => o.IsProject)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

        public async Task<RealEstateObject?> GetByIdAsync(int id)
            => await _db.RealEstateObjects
                .Include(o => o.Photos)
                .Include(o => o.Category)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<List<Category>> GetCategoriesAsync()
            => await _db.Categories.OrderBy(c => c.Name).ToListAsync();

        public async Task CreateAsync(RealEstateObject obj, List<IFormFile> photos)
        {
            obj.CreatedAt = DateTime.Now;
            _db.RealEstateObjects.Add(obj);
            await _db.SaveChangesAsync();
            await SavePhotosAsync(obj.Id, photos);
        }

        public async Task UpdateAsync(RealEstateObject obj)
        {
            _db.RealEstateObjects.Update(obj);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var obj = await _db.RealEstateObjects
                .Include(o => o.Photos)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (obj == null) return;
            foreach (var photo in obj.Photos)
                DeletePhysicalFile(photo.FilePath);
            _db.RealEstateObjects.Remove(obj);
            await _db.SaveChangesAsync();
        }

        public async Task<RealEstateObject> CopyAsync(int id)
        {
            var original = await _db.RealEstateObjects
                .FirstOrDefaultAsync(o => o.Id == id);
            if (original == null) throw new Exception("Об'єкт не знайдено");

            var copy = new RealEstateObject
            {
                CategoryId     = original.CategoryId,
                Title          = original.Title + " (копія)",
                Description    = original.Description,
                Price          = original.Price,
                PriceType      = original.PriceType,
                Area           = original.Area,
                Floor          = original.Floor,
                TotalFloors    = original.TotalFloors,
                Rooms          = original.Rooms,
                Address        = original.Address,
                BuildingStatus = original.BuildingStatus,
                SaleStatus     = "Available",
                IsHidden       = true,
                IsFeatured     = false,
                IsProject      = false,
                ProjectYear    = original.ProjectYear,
                ProjectQuarter = original.ProjectQuarter,
                CreatedAt      = DateTime.Now
            };

            _db.RealEstateObjects.Add(copy);
            await _db.SaveChangesAsync();
            return copy;
        }

        public async Task AddPhotosAsync(int objectId, List<IFormFile> photos)
            => await SavePhotosAsync(objectId, photos);

        public async Task SetMainPhotoAsync(int photoId, int objectId)
        {
            var photos = await _db.Photos
                .Where(p => p.ObjectId == objectId)
                .ToListAsync();
            foreach (var p in photos)
                p.IsMain = (p.Id == photoId);
            await _db.SaveChangesAsync();
        }

        public async Task DeletePhotoAsync(int photoId)
        {
            var photo = await _db.Photos.FindAsync(photoId);
            if (photo == null) return;
            DeletePhysicalFile(photo.FilePath);
            _db.Photos.Remove(photo);
            await _db.SaveChangesAsync();
        }

        private async Task SavePhotosAsync(int objectId, List<IFormFile> files)
        {
            if (files == null || !files.Any()) return;
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);
            bool isFirst = !await _db.Photos.AnyAsync(p => p.ObjectId == objectId);
            foreach (var file in files)
            {
                if (file.Length == 0) continue;
                var ext      = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var fullPath = Path.Combine(uploadsPath, fileName);
                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);
                _db.Photos.Add(new Photo
                {
                    ObjectId = objectId,
                    FilePath = $"/uploads/{fileName}",
                    AltText  = file.FileName,
                    IsMain   = isFirst
                });
                isFirst = false;
            }
            await _db.SaveChangesAsync();
        }

        private void DeletePhysicalFile(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath,
                filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
    }
}
