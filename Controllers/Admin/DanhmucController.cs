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
    public async Task<IActionResult> Index(string? tuKhoa, int? trangThai, int page = 1, int pageSize = 10)
    {
        ViewBag.TuKhoa    = tuKhoa;
        ViewBag.TrangThai = trangThai;

        var tatCa = await khoaHocService.LayDanhSachDanhMucAsync(tuKhoa);

        // Lọc trạng thái phía server
        if (trangThai.HasValue)
            tatCa = tatCa.Where(d => (int)d.TrangThai == trangThai.Value).ToList();

        ViewBag.Total    = tatCa.Count;
        ViewBag.Page     = page;
        ViewBag.PageSize = pageSize;

        var trang = tatCa.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return View("~/Views/Admin/DanhMuc/Index.cshtml", trang);
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

    // POST /Admin/DanhMuc/softdelete/5 — Xóa mềm
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> softdelete(int id)
    {
        var ketQua = await khoaHocService.XoaMemDanhMucAsync(id);
        TempData[ketQua ? "ThanhCong" : "LoiXay"] =
            ketQua ? "Đã chuyển danh mục vào thùng rác!" : "Không tìm thấy danh mục!";
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/DanhMuc/Trash — Thùng rác
    public async Task<IActionResult> Trash()
    {
        var danhSach = await khoaHocService.LayThuRacDanhMucAsync();
        return View("~/Views/Admin/DanhMuc/Trash.cshtml", danhSach);
    }

    // POST /Admin/DanhMuc/restore/5 — Khôi phục
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> restore(int id)
    {
        var ketQua = await khoaHocService.KhoiPhucDanhMucAsync(id);
        TempData[ketQua ? "ThanhCong" : "LoiXay"] =
            ketQua ? "Đã khôi phục danh mục thành công!" : "Không tìm thấy danh mục trong thùng rác!";
        return RedirectToAction(nameof(Trash));
    }
}
