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

public class KhoaHocController(IKhoaHocService khoaHocService) : Controller
{
    // GET /Admin/KhoaHoc
    // Hiển thị danh sách khóa học, có tìm kiếm và lọc theo danh mục
    public async Task<IActionResult> Index(string? tuKhoa, int? danhMucId)
    {
        // Lưu điều kiện tìm kiếm vào ViewBag để hiển thị lại trên form
        ViewBag.TuKhoa    = tuKhoa;
        ViewBag.DanhMucId = danhMucId;
        ViewBag.DanhMucs  = await khoaHocService.LayDanhMucAsync();

        var danhSach = await khoaHocService.LayDanhSachAsync(tuKhoa, danhMucId);
        return View(danhSach);
    }

    // GET /Admin/KhoaHoc/ChiTiet/5
    // Xem chi tiết 1 khóa học
    public async Task<IActionResult> ChiTiet(int id)
    {
        var khoaHoc = await khoaHocService.LayTheoIdAsync(id);
        if (khoaHoc == null) return NotFound("Không tìm thấy khóa học");
        return View(khoaHoc);
    }

    // GET /Admin/KhoaHoc/Them
    // Hiển thị form thêm khóa học mới
    public async Task<IActionResult> Them()
    {
        ViewBag.DanhMucs = await khoaHocService.LayDanhMucAsync();
        return View(new KhoaHoc());
    }

    // POST /Admin/KhoaHoc/Them
    // Xử lý form submit thêm khóa học
    [HttpPost]
    [ValidateAntiForgeryToken] // Bảo vệ khỏi tấn công CSRF
    public async Task<IActionResult> Them(KhoaHoc khoaHoc)
    {
        if (!ModelState.IsValid) // Kiểm tra validation từ Data Annotations
        {
            ViewBag.DanhMucs = await khoaHocService.LayDanhMucAsync();
            return View(khoaHoc); // Trả lại form với lỗi validation
        }

        // Tự tạo slug từ tên khóa học (sẽ bổ sung helper sau)
        khoaHoc.Slug = khoaHoc.TenKhoaHoc
            .ToLower()
            .Replace(" ", "-")
            .Replace("đ", "d");

        await khoaHocService.ThemAsync(khoaHoc);

        TempData["ThanhCong"] = "Đã thêm khóa học thành công!";
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/KhoaHoc/Sua/5
    // Hiển thị form sửa khóa học
    public async Task<IActionResult> Sua(int id)
    {
        var khoaHoc = await khoaHocService.LayTheoIdAsync(id);
        if (khoaHoc == null) return NotFound();
        ViewBag.DanhMucs = await khoaHocService.LayDanhMucAsync();
        return View(khoaHoc);
    }

    // POST /Admin/KhoaHoc/Sua
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sua(KhoaHoc khoaHoc)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.DanhMucs = await khoaHocService.LayDanhMucAsync();
            return View(khoaHoc);
        }

        var ketQua = await khoaHocService.CapNhatAsync(khoaHoc);
        if (!ketQua) return NotFound();

        TempData["ThanhCong"] = "Đã cập nhật khóa học!";
        return RedirectToAction(nameof(Index));
    }

    // POST /Admin/KhoaHoc/Xoa/5
    // Xóa mềm khóa học (soft delete)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Xoa(int id)
    {
        var ketQua = await khoaHocService.XoaMemAsync(id);
        TempData[ketQua ? "ThanhCong" : "LoiXay"] =
            ketQua ? "Đã xóa khóa học!" : "Không tìm thấy khóa học!";
        return RedirectToAction(nameof(Index));
    }
}
