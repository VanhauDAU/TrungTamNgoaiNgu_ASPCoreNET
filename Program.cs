// =============================================================================
// PROGRAM.CS — ĐIỂM KHỞI ĐẦU CỦA ỨNG DỤNG ASP.NET CORE
// =============================================================================
// Cấu trúc:  1. builder → đăng ký Services
//            2. app     → cấu hình pipeline HTTP request
// =============================================================================

using Microsoft.EntityFrameworkCore;
using TrungTamNgoaiNgu.Data;
using TrungTamNgoaiNgu.Services;
using TrungTamNgoaiNgu.Services.Interfaces;

// ===== BƯỚC 1: TẠO BUILDER =====
var builder = WebApplication.CreateBuilder(args);

// Đăng ký MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

// Kết nối SQL Server qua EF Core
// Đọc connection string từ appsettings.json → ConnectionStrings → DefaultConnection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== ĐĂNG KÝ SERVICES (Dependency Injection) =====
// AddScoped: tạo 1 instance mới cho mỗi HTTP request
// Controller khai báo interface trong constructor → .NET tự inject implementation
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IKhoaHocService,   KhoaHocService>();
builder.Services.AddScoped<ITaiChinhService,  TaiChinhService>();
builder.Services.AddScoped<IAuditLogsService, AuditLogsService>();

// ===== BƯỚC 2: XÂY DỰNG APP =====
var app = builder.Build();

// Tự apply migration khi khởi động để tránh lệch schema giữa code và DB
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// ===== BƯỚC 3: CẤU HÌNH PIPELINE HTTP =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // Phục vụ file trong wwwroot/
app.UseRouting();
app.UseAuthorization();

// ===== BƯỚC 4: ROUTING =====

// Admin route: /Admin/KhoaHoc/Index, /Admin/Dashboard...
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

// Public route: / → Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
