// =============================================================================
// ADMIN COURSE CATEGORIES CONTROLLER
// =============================================================================
// Quản lý CRUD danh mục khóa học: Xem danh sách, Thêm, Sửa, Xóa mềm
// URL pattern: Admin/CourseCategories/{action}
// =============================================================================
using Microsoft.AspNetCore.Mvc;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Controllers.Admin;

public class CourseCategoriesController(IKhoaHocService khoaHocService) : Controller
{
    // GET /Admin/CourseCategories
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
        return View("~/Views/Admin/CourseCategories/Index.cshtml", trang);
    }

    // GET /Admin/CourseCategories/Create
    public IActionResult Create()
    {
        return View("~/Views/Admin/CourseCategories/Create.cshtml", new DanhMucKhoaHoc());
    }

    // POST /Admin/CourseCategories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DanhMucKhoaHoc danhMuc)
    {
        // Loại bỏ validation cho Slug vì ta sẽ tự tạo nó ở Backend
        ModelState.Remove("Slug");

        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/CourseCategories/Create.cshtml", danhMuc);
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

    // GET /Admin/CourseCategories/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var danhMuc = await khoaHocService.LayDanhMucTheoIdAsync(id);
        if (danhMuc == null) return NotFound();
        
        return View("~/Views/Admin/CourseCategories/Edit.cshtml", danhMuc);
    }

    // POST /Admin/CourseCategories/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(DanhMucKhoaHoc danhMuc)
    {
        ModelState.Remove("Slug");
        
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/CourseCategories/Edit.cshtml", danhMuc);
        }

        var ketQua = await khoaHocService.CapNhatDanhMucAsync(danhMuc);
        if (!ketQua) return NotFound();

        TempData["ThanhCong"] = "Đã cập nhật danh mục!";
        return RedirectToAction(nameof(Index));
    }

    // POST /Admin/CourseCategories/softdelete/5 — Xóa mềm
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> softdelete(int id)
    {
        var ketQua = await khoaHocService.XoaMemDanhMucAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/CourseCategories/Trash — Thùng rác
    public async Task<IActionResult> Trash()
    {
        var danhSach = await khoaHocService.LayThuRacDanhMucAsync();
        return View("~/Views/Admin/CourseCategories/Trash.cshtml", danhSach);
    }

    // POST /Admin/CourseCategories/restore/5 — Khôi phục
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> restore(int id)
    {
        var ketQua = await khoaHocService.KhoiPhucDanhMucAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Trash));
    }

    private string LayNguoiThucHien()
    {
        if (User?.Identity?.IsAuthenticated == true && !string.IsNullOrWhiteSpace(User.Identity.Name))
            return User.Identity.Name!;
        return "Quản trị viên";
    }
}
