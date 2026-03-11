// =============================================================================
// ADMIN CLASSES CONTROLLER
// =============================================================================
// Quản lý lớp học CRUD + state-machine + soft delete + bulk actions
// URL pattern: /Admin/Classes/{action}
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Controllers.Admin;

public class ClassesController(IClassesService classesService) : Controller
{
    // =========================================================================
    // INDEX — Danh sách + phân trang + lọc
    // =========================================================================
    public async Task<IActionResult> Index(
        int? khoaHocId, int? coSoId, int? trangThai,
        string? tuKhoa, int page = 1, int pageSize = 10)
    {
        ViewBag.KhoaHocId = khoaHocId;
        ViewBag.CoSoId    = coSoId;
        ViewBag.TrangThai = trangThai;
        ViewBag.TuKhoa    = tuKhoa;

        ViewBag.Stats         = await classesService.LayThongKeAsync();
        ViewBag.KhoaHocs      = await classesService.LayKhoaHocDropdownAsync();
        ViewBag.CoSos         = await classesService.LayCoSoDropdownAsync();

        var ketQua = await classesService.LayDanhSachPhanTrangAsync(
            khoaHocId, coSoId, trangThai, tuKhoa, page, pageSize);

        ViewBag.Total    = ketQua.Total;
        ViewBag.Page     = ketQua.Page;
        ViewBag.PageSize = ketQua.PageSize;

        return View("~/Views/Admin/Classes/Index.cshtml", ketQua.Items);
    }

    // =========================================================================
    // DETAIL — Chi tiết lớp học + học viên + buổi học
    // =========================================================================
    public async Task<IActionResult> Detail(int id)
    {
        var lopHoc = await classesService.LayTheoIdAsync(id);
        if (lopHoc == null) return NotFound("Không tìm thấy lớp học.");

        ViewBag.HocViens  = await classesService.LayHocVienTrongLopAsync(id);
        ViewBag.BuoiHocs  = await classesService.LayBuoiHocAsync(id);
        return View("~/Views/Admin/Classes/Detail.cshtml", lopHoc);
    }

    // =========================================================================
    // CREATE — Tạo lớp học mới
    // =========================================================================
    public async Task<IActionResult> Create()
    {
        await NapDropdowns();
        return View("~/Views/Admin/Classes/Create.cshtml", new LopHoc());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LopHoc lopHoc)
    {
        ModelState.Remove("Slug");

        if (!ModelState.IsValid)
        {
            await NapDropdowns(lopHoc.KhoaHocId, lopHoc.CoSoId);
            return View("~/Views/Admin/Classes/Create.cshtml", lopHoc);
        }

        lopHoc.Slug = await classesService.TaoSlugLopHocAsync(lopHoc.TenLopHoc ?? "lop-hoc");

        var ketQua = await classesService.ThemAsync(lopHoc, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Index));
    }

    // =========================================================================
    // EDIT — Sửa lớp học
    // =========================================================================
    public async Task<IActionResult> Edit(int id)
    {
        var lopHoc = await classesService.LayTheoIdAsync(id);
        if (lopHoc == null) return NotFound();

        await NapDropdowns(lopHoc.KhoaHocId, lopHoc.CoSoId);
        return View("~/Views/Admin/Classes/Edit.cshtml", lopHoc);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(LopHoc lopHoc)
    {
        ModelState.Remove("Slug");

        if (!ModelState.IsValid)
        {
            await NapDropdowns(lopHoc.KhoaHocId, lopHoc.CoSoId);
            return View("~/Views/Admin/Classes/Edit.cshtml", lopHoc);
        }

        lopHoc.Slug = await classesService.TaoSlugLopHocAsync(
            lopHoc.TenLopHoc ?? "lop-hoc", lopHoc.LopHocId);

        var ketQua = await classesService.CapNhatAsync(lopHoc, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;

        if (!ketQua.ThanhCong)
        {
            await NapDropdowns(lopHoc.KhoaHocId, lopHoc.CoSoId);
            return View("~/Views/Admin/Classes/Edit.cshtml", lopHoc);
        }

        return RedirectToAction(nameof(Index));
    }

    // =========================================================================
    // CHANGE STATUS — Chuyển trạng thái (AJAX + form POST)
    // =========================================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> changestatus(int id, byte trangThaiMoi, string? returnUrl = null)
    {
        var ketQua = await classesService.ChuyenTrangThaiAsync(id, trangThaiMoi, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    // =========================================================================
    // SOFT DELETE
    // =========================================================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> softdelete(int id)
    {
        var ketQua = await classesService.XoaMemAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Index));
    }

    // =========================================================================
    // TRASH — Thùng rác
    // =========================================================================
    public async Task<IActionResult> Trash()
    {
        var danhSach = await classesService.LayThuRacAsync();
        return View("~/Views/Admin/Classes/Trash.cshtml", danhSach);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> restore(int id)
    {
        var ketQua = await classesService.KhoiPhucAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Trash));
    }

    // =========================================================================
    // AJAX ENDPOINTS — Dropdowns động
    // =========================================================================

    /// <summary>GET /Admin/Classes/phonghoc-by-coso?coSoId=1</summary>
    [HttpGet]
    public async Task<IActionResult> PhongHocByCoso(int? coSoId)
    {
        var phongHocs = await classesService.LayPhongHocDropdownAsync(coSoId);
        return Json(phongHocs.Select(p => new { id = p.PhongHocId, name = p.TenPhong }));
    }

    /// <summary>GET /Admin/Classes/hocphi-by-khoahoc?khoaHocId=1</summary>
    [HttpGet]
    public async Task<IActionResult> HocPhiByKhoaHoc(int? khoaHocId)
    {
        var hocPhis = await classesService.LayHocPhiDropdownAsync(khoaHocId);
        return Json(hocPhis.Select(h => new { id = h.HocPhiId, name = $"{h.SoBuoi} buổi - {h.DonGia?.ToString("N0") ?? "?"}đ/buổi" }));
    }

    // =========================================================================
    // PRIVATE HELPERS
    // =========================================================================

    private async Task NapDropdowns(int? khoaHocId = null, int? coSoId = null)
    {
        ViewBag.KhoaHocs   = await classesService.LayKhoaHocDropdownAsync();
        ViewBag.CaHocs     = await classesService.LayCaHocDropdownAsync();
        ViewBag.CoSos      = await classesService.LayCoSoDropdownAsync();
        ViewBag.PhongHocs  = await classesService.LayPhongHocDropdownAsync(coSoId);
        ViewBag.GiaoViens  = await classesService.LayGiaoVienDropdownAsync();
        ViewBag.HocPhis    = await classesService.LayHocPhiDropdownAsync(khoaHocId);
    }

    private string LayNguoiThucHien()
        => User?.Identity?.IsAuthenticated == true && !string.IsNullOrWhiteSpace(User.Identity.Name)
            ? User.Identity.Name!
            : "Quản trị viên";
}
