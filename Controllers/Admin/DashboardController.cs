// =============================================================================
// ADMIN DASHBOARD CONTROLLER (Đã refactor dùng Service)
// =============================================================================
// ✅ Controller chỉ: nhận request → gọi service → trả về View
// ✅ KHÔNG chứa logic nghiệp vụ hay query database trực tiếp
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Controllers.Admin;

public class DashboardController(IDashboardService dashboardService) : Controller
{
    // IDashboardService được .NET inject tự động (không cần new)
    // → Đã đăng ký trong Program.cs: builder.Services.AddScoped<IDashboardService, DashboardService>()

    // GET /Admin/Dashboard/Index (hoặc /Admin)
    public async Task<IActionResult> Index()
    {
        var thongKe = await dashboardService.LayThongKeAsync();
        // Chỉ định rõ đường dẫn vì controller nằm trong namespace con Admin
        return View("~/Views/Admin/Dashboard/Index.cshtml", thongKe);
    }
}
