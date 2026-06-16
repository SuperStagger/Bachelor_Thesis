// Файл: Program.cs
using Microsoft.EntityFrameworkCore;
using ConstructionCompany.Data;
using ConstructionCompany.Services;
using ConstructionCompany.Services.Interfaces;

// ── Глобальне налаштування культури ──────────────────────────
// Вирішує проблему з decimal (крапка замість коми) раз і назавжди
var cultureInfo = System.Globalization.CultureInfo.InvariantCulture;
System.Globalization.CultureInfo.DefaultThreadCurrentCulture   = cultureInfo;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// ── 1. База даних ─────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── 2. Сервіси (Dependency Injection) ────────────────────────
builder.Services.AddScoped<IRealEstateService,    RealEstateService>();
builder.Services.AddScoped<IBookingService,       BookingService>();
builder.Services.AddScoped<IUserService,          UserService>();
builder.Services.AddScoped<INotificationService,  NotificationService>();

// ── 3. Сесія ─────────────────────────────────────────────────
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout        = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly    = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name        = ".ConstructionCompany.Session";
});

// ── 4. Razor Pages ────────────────────────────────────────────
builder.Services.AddRazorPages();

var app = builder.Build();

// ── 5. Middleware ─────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapRazorPages();

// ── 6. Автоматичне застосування міграцій ─────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();
