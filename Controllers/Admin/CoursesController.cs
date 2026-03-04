// =============================================================================
// ADMIN COURSES CONTROLLER
// =============================================================================
// Quản lý CRUD khóa học: Xem danh sách, Thêm, Sửa, Xóa
// URL pattern: /Admin/Courses/{action}
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Controllers.Admin;

public class CoursesController(ICoursesService courseService, IWebHostEnvironment env) : Controller
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private static readonly HashSet<string> AllowedImageMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg", "image/png", "image/webp"
    };
    private const long MaxImageSizeBytes = 5 * 1024 * 1024;

    // GET /Admin/Courses
    public async Task<IActionResult> Index(string? tuKhoa, int? danhMucId, int? trangThai, int page = 1, int pageSize = 10)
    {
        ViewBag.TuKhoa    = tuKhoa;
        ViewBag.DanhMucId = danhMucId;
        ViewBag.TrangThai = trangThai;
        ViewBag.DanhMucs  = await courseService.LayDanhMucAsync();
        ViewBag.Stats     = await courseService.LayThongKeQuanLyAsync();

        var ketQua = await courseService.LayDanhSachPhanTrangAsync(tuKhoa, danhMucId, trangThai, page, pageSize);

        ViewBag.Total    = ketQua.Total;
        ViewBag.Page     = ketQua.Page;
        ViewBag.PageSize = ketQua.PageSize;

        return View("~/Views/Admin/Courses/Index.cshtml", ketQua.Items);
    }

    // GET /Admin/Courses/Detail/5
    // Xem chi tiết 1 khóa học
    public async Task<IActionResult> Detail(int id)
    {
        var khoaHoc = await courseService.LayTheoIdAsync(id);
        if (khoaHoc == null) return NotFound("Không tìm thấy khóa học");
        return View("~/Views/Admin/Courses/Detail.cshtml", khoaHoc);
    }

    // GET /Admin/Courses/Create
    // Hiển thị form thêm khóa học mới
    public async Task<IActionResult> Create()
    {
        ViewBag.DanhMucs = await courseService.LayDanhMucAsync();
        return View("~/Views/Admin/Courses/Create.cshtml", new KhoaHoc());
    }

    // POST /Admin/Courses/Create
    // Xử lý form submit thêm khóa học
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(KhoaHoc khoaHoc, IFormFile? anhFile)
    {
        // Slug được tạo server-side, không cần validate từ client
        ModelState.Remove("Slug");
        ModelState.Remove("AnhKhoaHoc");
        KiemTraAnhUpload(anhFile);

        if (!ModelState.IsValid)
        {
            ViewBag.DanhMucs = await courseService.LayDanhMucAsync();
            return View("~/Views/Admin/Courses/Create.cshtml", khoaHoc);
        }

        // Xử lý upload ảnh nếu có
        khoaHoc.AnhKhoaHoc = await LuuAnhKhoaHocAsync(anhFile);

        // Tạo slug chuẩn + chống trùng
        khoaHoc.Slug = await courseService.TaoSlugKhoaHocAsync(khoaHoc.TenKhoaHoc);

        await courseService.ThemAsync(khoaHoc, LayNguoiThucHien());

        TempData["ThanhCong"] = "Đã thêm khóa học thành công!";
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/Courses/Edit/5
    // Hiển thị form sửa khóa học
    public async Task<IActionResult> Edit(int id)
    {
        var khoaHoc = await courseService.LayTheoIdAsync(id);
        if (khoaHoc == null) return NotFound();
        ViewBag.DanhMucs = await courseService.LayDanhMucAsync();
        return View("~/Views/Admin/Courses/Edit.cshtml", khoaHoc);
    }

    // POST /Admin/Courses/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(KhoaHoc khoaHoc, IFormFile? anhFile)
    {
        ModelState.Remove("Slug");
        ModelState.Remove("AnhKhoaHoc");
        KiemTraAnhUpload(anhFile);

        if (!ModelState.IsValid)
        {
            ViewBag.DanhMucs = await courseService.LayDanhMucAsync();
            return View("~/Views/Admin/Courses/Edit.cshtml", khoaHoc);
        }

        // Xử lý upload ảnh mới nếu có
        var anhMoi = await LuuAnhKhoaHocAsync(anhFile);
        if (!string.IsNullOrEmpty(anhMoi))
        {
            khoaHoc.AnhKhoaHoc = anhMoi;
        }

        // Cập nhật slug theo tên mới (tránh trùng slug khóa học khác)
        khoaHoc.Slug = await courseService.TaoSlugKhoaHocAsync(khoaHoc.TenKhoaHoc, khoaHoc.KhoaHocId);

        var ketQua = await courseService.CapNhatCoKiemTraAsync(khoaHoc, LayNguoiThucHien());
        if (!ketQua.ThanhCong)
        {
            ModelState.AddModelError("TrangThai", ketQua.ThongBao);
            ViewBag.DanhMucs = await courseService.LayDanhMucAsync();
            return View("~/Views/Admin/Courses/Edit.cshtml", khoaHoc);
        }

        TempData["ThanhCong"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Index));
    }

    // POST /Admin/Courses/softdelete/5 — Xóa mềm
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> softdelete(int id)
    {
        var ketQua = await courseService.XoaMemAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/Courses/Trash — Thùng rác
    public async Task<IActionResult> Trash()
    {
        var danhSach = await courseService.LayThuRacAsync();
        return View("~/Views/Admin/Courses/Trash.cshtml", danhSach);
    }

    // POST /Admin/Courses/restore/5 — Khôi phục
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> restore(int id)
    {
        var ketQua = await courseService.KhoiPhucAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Trash));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> bulk(List<int> selectedIds, string? bulkAction, byte? bulkTrangThai)
    {
        ServiceResult ketQua;
        switch (bulkAction)
        {
            case "change-status":
                if (!bulkTrangThai.HasValue)
                {
                    TempData["LoiXay"] = "Vui lòng chọn trạng thái cần cập nhật.";
                    return RedirectToAction(nameof(Index));
                }
                ketQua = await courseService.DoiTrangThaiHangLoatAsync(selectedIds, bulkTrangThai.Value, LayNguoiThucHien());
                break;
            case "soft-delete":
                ketQua = await courseService.XoaMemHangLoatAsync(selectedIds, LayNguoiThucHien());
                break;
            default:
                TempData["LoiXay"] = "Hành động không hợp lệ.";
                return RedirectToAction(nameof(Index));
        }

        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> bulkrestore(List<int> selectedIds)
    {
        var ketQua = await courseService.KhoiPhucHangLoatAsync(selectedIds, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Trash));
    }

    private void KiemTraAnhUpload(IFormFile? anhFile)
    {
        if (anhFile == null || anhFile.Length == 0) return;

        if (anhFile.Length > MaxImageSizeBytes)
        {
            ModelState.AddModelError("AnhKhoaHoc", "Ảnh vượt quá 5MB.");
            return;
        }

        var extension = Path.GetExtension(anhFile.FileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(extension))
        {
            ModelState.AddModelError("AnhKhoaHoc", "Chỉ cho phép ảnh JPG, JPEG, PNG hoặc WEBP.");
            return;
        }

        if (!AllowedImageMimeTypes.Contains(anhFile.ContentType))
        {
            ModelState.AddModelError("AnhKhoaHoc", "Định dạng MIME của ảnh không hợp lệ.");
        }
    }

    private async Task<string?> LuuAnhKhoaHocAsync(IFormFile? anhFile)
    {
        if (anhFile == null || anhFile.Length == 0) return null;

        var uploadDir = Path.Combine(env.WebRootPath, "uploads", "khoahoc");
        Directory.CreateDirectory(uploadDir);

        var extension = Path.GetExtension(anhFile.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid()}{extension}";
        var outputPath = Path.Combine(uploadDir, fileName);

        await using var stream = new FileStream(outputPath, FileMode.Create);
        await anhFile.CopyToAsync(stream);

        return $"/uploads/khoahoc/{fileName}";
    }

    private string LayNguoiThucHien()
    {
        if (User?.Identity?.IsAuthenticated == true && !string.IsNullOrWhiteSpace(User.Identity.Name))
            return User.Identity.Name!;
        return "Quản trị viên";
    }
}
