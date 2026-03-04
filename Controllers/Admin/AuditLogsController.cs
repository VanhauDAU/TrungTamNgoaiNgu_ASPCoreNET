using Microsoft.AspNetCore.Mvc;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Controllers.Admin;

public class AuditLogsController(IAuditLogsService auditLogsService) : Controller
{
    // GET: /Admin/AuditLogs
    public async Task<IActionResult> Index(string? module, string? tuKhoa, int page = 1, int pageSize = 20)
    {
        ViewBag.Module = module;
        ViewBag.TuKhoa = tuKhoa;
        ViewBag.Modules = await auditLogsService.LayDanhSachModuleAsync();

        var ketQua = await auditLogsService.LayDanhSachPhanTrangAsync(module, tuKhoa, page, pageSize);
        ViewBag.Total = ketQua.Total;
        ViewBag.Page = ketQua.Page;
        ViewBag.PageSize = ketQua.PageSize;

        return View("~/Views/Admin/AuditLogs/Index.cshtml", ketQua.Items);
    }

    // GET: /Admin/AuditLogs/Details/5
    public async Task<IActionResult> Details(long id, string? returnUrl = null)
    {
        var log = await auditLogsService.LayTheoIdAsync(id);
        if (log == null) return NotFound("Không tìm thấy bản ghi nhật ký.");

        ViewBag.ReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Action(nameof(Index));
        return View("~/Views/Admin/AuditLogs/Details.cshtml", log);
    }
}
