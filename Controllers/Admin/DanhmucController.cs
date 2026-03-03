// =============================================================================
// ADMIN DANHMUC CONTROLLER
// =============================================================================
// Quản lý CRUD danh mục khóa học: Xem danh sách, Thêm, Sửa, Xóa mềm
// URL pattern: Admin/DanhMuc/{action}
// =============================================================================
using Microsoft.AspNetCore.Mvc;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Controllers.Admin;

public class DanhMucController(IKhoaHocService khoaHocService) : Controller
{
    // GET /Admin/DanhMuc
    public async Task<IActionResult> Index(string? tuKhoa)
    {
        ViewBag.TuKhoa = tuKhoa;
        var danhSach = await khoaHocService.LayDanhSachDanhMucAsync(tuKhoa);
        return View("~/Views/Admin/DanhMuc/Index.cshtml", danhSach);
    }

    // GET /Admin/DanhMuc/Create
    public IActionResult Create()
    {
        return View("~/Views/Admin/DanhMuc/Create.cshtml", new DanhMucKhoaHoc());
    }

    // POST /Admin/DanhMuc/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DanhMucKhoaHoc danhMuc)
    {
        // Loại bỏ validation cho Slug vì ta sẽ tự tạo nó ở Backend
        ModelState.Remove("Slug");

        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/DanhMuc/Create.cshtml", danhMuc);
        }

        // Tự tạo slug từ tên danh mục
        danhMuc.Slug = danhMuc.TenDanhMuc
            .ToLower()
            .Replace(" ", "-")
            .Replace("đ", "d"); // Basic mock formatting

        await khoaHocService.ThemDanhMucAsync(danhMuc);

        TempData["ThanhCong"] = "Đã thêm danh mục thành công!";
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/DanhMuc/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var danhMuc = await khoaHocService.LayDanhMucTheoIdAsync(id);
        if (danhMuc == null) return NotFound();
        
        return View("~/Views/Admin/DanhMuc/Edit.cshtml", danhMuc);
    }

    // POST /Admin/DanhMuc/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(DanhMucKhoaHoc danhMuc)
    {
        ModelState.Remove("Slug");
        
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/DanhMuc/Edit.cshtml", danhMuc);
        }

        var ketQua = await khoaHocService.CapNhatDanhMucAsync(danhMuc);
        if (!ketQua) return NotFound();

        TempData["ThanhCong"] = "Đã cập nhật danh mục!";
        return RedirectToAction(nameof(Index));
    }

    // POST /Admin/DanhMuc/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var ketQua = await khoaHocService.XoaMemDanhMucAsync(id);
        TempData[ketQua ? "ThanhCong" : "LoiXay"] = 
            ketQua ? "Đã xóa danh mục khóa học!" : "Không tìm thấy danh mục!";
        return RedirectToAction(nameof(Index));
    }
}
