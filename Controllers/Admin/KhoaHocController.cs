// =============================================================================
// ADMIN KHOA HOC CONTROLLER
// =============================================================================
// Quản lý CRUD khóa học: Xem danh sách, Thêm, Sửa, Xóa
// URL pattern: /Admin/KhoaHoc/{action}
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Controllers.Admin;

public class KhoaHocController(IKhoaHocService khoaHocService, IWebHostEnvironment env) : Controller
{
    // GET /Admin/KhoaHoc
    public async Task<IActionResult> Index(string? tuKhoa, int? danhMucId, int? trangThai, int page = 1, int pageSize = 10)
    {
        ViewBag.TuKhoa    = tuKhoa;
        ViewBag.DanhMucId = danhMucId;
        ViewBag.TrangThai = trangThai;
        ViewBag.DanhMucs  = await khoaHocService.LayDanhMucAsync();

        var tatCa = await khoaHocService.LayDanhSachAsync(tuKhoa, danhMucId);

        // Lọc trạng thái phía server
        if (trangThai.HasValue)
            tatCa = tatCa.Where(k => (int)k.TrangThai == trangThai.Value).ToList();

        ViewBag.Total    = tatCa.Count;
        ViewBag.Page     = page;
        ViewBag.PageSize = pageSize;

        var trang = tatCa.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return View("~/Views/Admin/KhoaHoc/Index.cshtml", trang);
    }

    // GET /Admin/KhoaHoc/ChiTiet/5
    // Xem chi tiết 1 khóa học
    public async Task<IActionResult> ChiTiet(int id)
    {
        var khoaHoc = await khoaHocService.LayTheoIdAsync(id);
        if (khoaHoc == null) return NotFound("Không tìm thấy khóa học");
        return View("~/Views/Admin/KhoaHoc/ChiTiet.cshtml", khoaHoc);
    }

    // GET /Admin/KhoaHoc/Create
    // Hiển thị form thêm khóa học mới
    public async Task<IActionResult> Create()
    {
        ViewBag.DanhMucs = await khoaHocService.LayDanhMucAsync();
        return View("~/Views/Admin/KhoaHoc/Create.cshtml", new KhoaHoc());
    }

    // POST /Admin/KhoaHoc/Create
    // Xử lý form submit thêm khóa học
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(KhoaHoc khoaHoc, IFormFile? anhFile)
    {
        // Slug được tạo server-side, không cần validate từ client
        ModelState.Remove("Slug");
        ModelState.Remove("AnhKhoaHoc");

        if (!ModelState.IsValid)
        {
            ViewBag.DanhMucs = await khoaHocService.LayDanhMucAsync();
            return View("~/Views/Admin/KhoaHoc/Create.cshtml", khoaHoc);
        }

        // Xử lý upload ảnh nếu có
        if (anhFile != null && anhFile.Length > 0)
        {
            var uploadDir = Path.Combine(env.WebRootPath, "uploads", "khoahoc");
            Directory.CreateDirectory(uploadDir);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(anhFile.FileName)}";
            using var stream = new FileStream(Path.Combine(uploadDir, fileName), FileMode.Create);
            await anhFile.CopyToAsync(stream);
            khoaHoc.AnhKhoaHoc = $"/uploads/khoahoc/{fileName}";
        }

        // Tự tạo slug từ tên khóa học
        khoaHoc.Slug = khoaHoc.TenKhoaHoc
            .ToLower()
            .Replace(" ", "-")
            .Replace("đ", "d");

        await khoaHocService.ThemAsync(khoaHoc);

        TempData["ThanhCong"] = "Đã thêm khóa học thành công!";
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/KhoaHoc/Edit/5
    // Hiển thị form sửa khóa học
    public async Task<IActionResult> Edit(int id)
    {
        var khoaHoc = await khoaHocService.LayTheoIdAsync(id);
        if (khoaHoc == null) return NotFound();
        ViewBag.DanhMucs = await khoaHocService.LayDanhMucAsync();
        return View("~/Views/Admin/KhoaHoc/Edit.cshtml", khoaHoc);
    }

    // POST /Admin/KhoaHoc/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(KhoaHoc khoaHoc, IFormFile? anhFile)
    {
        ModelState.Remove("Slug");
        ModelState.Remove("AnhKhoaHoc");

        if (!ModelState.IsValid)
        {
            ViewBag.DanhMucs = await khoaHocService.LayDanhMucAsync();
            return View("~/Views/Admin/KhoaHoc/Edit.cshtml", khoaHoc);
        }

        // Xử lý upload ảnh mới nếu có
        if (anhFile != null && anhFile.Length > 0)
        {
            var uploadDir = Path.Combine(env.WebRootPath, "uploads", "khoahoc");
            Directory.CreateDirectory(uploadDir);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(anhFile.FileName)}";
            using var stream = new FileStream(Path.Combine(uploadDir, fileName), FileMode.Create);
            await anhFile.CopyToAsync(stream);
            khoaHoc.AnhKhoaHoc = $"/uploads/khoahoc/{fileName}";
        }

        // Cập nhật slug theo tên mới
        khoaHoc.Slug = khoaHoc.TenKhoaHoc
            .ToLower()
            .Replace(" ", "-")
            .Replace("đ", "d");

        var ketQua = await khoaHocService.CapNhatAsync(khoaHoc);
        if (!ketQua) return NotFound();

        TempData["ThanhCong"] = "Đã cập nhật khóa học!";
        return RedirectToAction(nameof(Index));
    }

    // POST /Admin/KhoaHoc/softdelete/5 — Xóa mềm
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> softdelete(int id)
    {
        var ketQua = await khoaHocService.XoaMemAsync(id);
        TempData[ketQua ? "ThanhCong" : "LoiXay"] =
            ketQua ? "Đã chuyển khóa học vào thùng rác!" : "Không tìm thấy khóa học!";
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/KhoaHoc/Trash — Thùng rác
    public async Task<IActionResult> Trash()
    {
        var danhSach = await khoaHocService.LayThuRacAsync();
        return View("~/Views/Admin/KhoaHoc/Trash.cshtml", danhSach);
    }

    // POST /Admin/KhoaHoc/restore/5 — Khôi phục
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> restore(int id)
    {
        var ketQua = await khoaHocService.KhoiPhucAsync(id);
        TempData[ketQua ? "ThanhCong" : "LoiXay"] =
            ketQua ? "Đã khôi phục khóa học thành công!" : "Không tìm thấy khóa học trong thùng rác!";
        return RedirectToAction(nameof(Trash));
    }
}
