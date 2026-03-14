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

public class CourseCategoriesController(ICoursesService courseService) : Controller
{
    // GET /Admin/CourseCategories
    public async Task<IActionResult> Index(string? tuKhoa, int? trangThai, int page = 1, int pageSize = 10)
    {
        ViewBag.TuKhoa    = tuKhoa;
        ViewBag.TrangThai = trangThai;

        var tatCa = await courseService.LayDanhSachDanhMucAsync(tuKhoa);

        // Lọc trạng thái phía server
        if (trangThai.HasValue)
            tatCa = tatCa.Where(d => (int)d.TrangThai == trangThai.Value).ToList();

        var duLieu = TaoDanhSachDanhMucHienThi(tatCa);

        ViewBag.Total    = duLieu.Count;
        ViewBag.Page     = page;
        ViewBag.PageSize = pageSize;

        var trang = duLieu.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return View("~/Views/Admin/CourseCategories/Index.cshtml", new CourseCategoryIndexViewModel
        {
            Items = trang,
            ThongKe = await courseService.LayThongKeDanhMucAsync()
        });
    }

    // GET /Admin/CourseCategories/Create
    public async Task<IActionResult> Create()
    {
        return View("~/Views/Admin/CourseCategories/Create.cshtml",
            await TaoFormViewModelAsync(new DanhMucKhoaHoc { TrangThai = 1 }));
    }

    // POST /Admin/CourseCategories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CourseCategoryFormViewModel model)
    {
        ModelState.Remove("Form.Slug");

        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/CourseCategories/Create.cshtml", await TaoFormViewModelAsync(model.Form));
        }

        var ketQua = await courseService.ThemDanhMucAsync(model.Form, LayNguoiThucHien());
        if (!ketQua.ThanhCong)
        {
            ModelState.AddModelError(string.Empty, ketQua.ThongBao);
            return View("~/Views/Admin/CourseCategories/Create.cshtml", await TaoFormViewModelAsync(model.Form));
        }

        TempData["ThanhCong"] = "Đã thêm danh mục thành công!";
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/CourseCategories/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var danhMuc = await courseService.LayDanhMucTheoIdAsync(id);
        if (danhMuc == null) return NotFound();
        
        return View("~/Views/Admin/CourseCategories/Edit.cshtml", await TaoFormViewModelAsync(danhMuc));
    }

    // POST /Admin/CourseCategories/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CourseCategoryFormViewModel model)
    {
        ModelState.Remove("Form.Slug");
        
        if (!ModelState.IsValid)
        {
            return View("~/Views/Admin/CourseCategories/Edit.cshtml", await TaoFormViewModelAsync(model.Form));
        }

        var ketQua = await courseService.CapNhatDanhMucAsync(model.Form, LayNguoiThucHien());
        if (!ketQua.ThanhCong)
        {
            ModelState.AddModelError(string.Empty, ketQua.ThongBao);
            return View("~/Views/Admin/CourseCategories/Edit.cshtml", await TaoFormViewModelAsync(model.Form));
        }

        TempData["ThanhCong"] = "Đã cập nhật danh mục!";
        return RedirectToAction(nameof(Index));
    }

    // POST /Admin/CourseCategories/softdelete/5 — Xóa mềm
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> softdelete(int id)
    {
        var ketQua = await courseService.XoaMemDanhMucAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/CourseCategories/Trash — Thùng rác
    public async Task<IActionResult> Trash()
    {
        var danhSach = await courseService.LayThuRacDanhMucAsync();
        return View("~/Views/Admin/CourseCategories/Trash.cshtml", danhSach);
    }

    // POST /Admin/CourseCategories/restore/5 — Khôi phục
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> restore(int id)
    {
        var ketQua = await courseService.KhoiPhucDanhMucAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Trash));
    }

    private string LayNguoiThucHien()
    {
        if (User?.Identity?.IsAuthenticated == true && !string.IsNullOrWhiteSpace(User.Identity.Name))
            return User.Identity.Name!;
        return "Quản trị viên";
    }

    private async Task<CourseCategoryFormViewModel> TaoFormViewModelAsync(DanhMucKhoaHoc form)
    {
        var tatCa = await courseService.LayDanhSachDanhMucAsync();
        var current = form.DanhMucId > 0
            ? tatCa.FirstOrDefault(dm => dm.DanhMucId == form.DanhMucId)
            : null;

        return new CourseCategoryFormViewModel
        {
            Form = form,
            SlugPreview = !string.IsNullOrWhiteSpace(form.Slug)
                ? form.Slug
                : await courseService.TaoSlugDanhMucAsync(form.TenDanhMuc, form.DanhMucId > 0 ? form.DanhMucId : null),
            ParentOptions = TaoDanhMucChaOptions(tatCa, form.DanhMucId),
            SoDanhMucCon = current?.Children.Count ?? tatCa.Count(dm => dm.ParentId == form.DanhMucId),
            SoKhoaHocTong = current?.KhoaHocs.Count ?? 0,
            SoKhoaHocDangHoatDong = current?.KhoaHocs.Count(k => k.TrangThai != 0) ?? 0
        };
    }

    private static List<CourseCategoryListItemViewModel> TaoDanhSachDanhMucHienThi(List<DanhMucKhoaHoc> tatCa)
    {
        var ketQua = new List<CourseCategoryListItemViewModel>();
        var byParent = tatCa
            .GroupBy(dm => dm.ParentId ?? 0)
            .ToDictionary(
                group => group.Key,
                group => group.OrderBy(dm => dm.SortOrder).ThenBy(dm => dm.TenDanhMuc).ToList());

        void Duyet(int parentId, int capDo, string duongDan)
        {
            if (!byParent.TryGetValue(parentId, out var children)) return;

            foreach (var item in children)
            {
                var currentPath = string.IsNullOrWhiteSpace(duongDan)
                    ? item.TenDanhMuc
                    : $"{duongDan} / {item.TenDanhMuc}";

                ketQua.Add(new CourseCategoryListItemViewModel
                {
                    DanhMuc = item,
                    CapDo = capDo,
                    TenHienThi = $"{string.Concat(Enumerable.Repeat("-- ", capDo))}{item.TenDanhMuc}",
                    DuongDanCha = duongDan,
                    SoDanhMucCon = byParent.GetValueOrDefault(item.DanhMucId)?.Count ?? 0,
                    SoKhoaHocTong = item.KhoaHocs?.Count ?? 0,
                    SoKhoaHocDangHoatDong = item.KhoaHocs?.Count(k => k.TrangThai != 0) ?? 0
                });

                Duyet(item.DanhMucId, capDo + 1, currentPath);
            }
        }

        Duyet(0, 0, string.Empty);

        foreach (var orphan in tatCa
                     .Where(dm => dm.ParentId.HasValue && !tatCa.Any(parent => parent.DanhMucId == dm.ParentId.Value))
                     .OrderBy(dm => dm.SortOrder)
                     .ThenBy(dm => dm.TenDanhMuc))
        {
            if (ketQua.Any(item => item.DanhMuc.DanhMucId == orphan.DanhMucId)) continue;

            ketQua.Add(new CourseCategoryListItemViewModel
            {
                DanhMuc = orphan,
                CapDo = 0,
                TenHienThi = orphan.TenDanhMuc,
                DuongDanCha = "Danh mục cha không còn tồn tại",
                SoDanhMucCon = byParent.GetValueOrDefault(orphan.DanhMucId)?.Count ?? 0,
                SoKhoaHocTong = orphan.KhoaHocs?.Count ?? 0,
                SoKhoaHocDangHoatDong = orphan.KhoaHocs?.Count(k => k.TrangThai != 0) ?? 0
            });
        }

        return ketQua;
    }

    private static List<CourseCategoryOptionViewModel> TaoDanhMucChaOptions(List<DanhMucKhoaHoc> tatCa, int danhMucIdHienTai)
    {
        var idCanLoai = danhMucIdHienTai > 0
            ? LayTatCaDanhMucConIds(tatCa, danhMucIdHienTai)
            : [];

        if (danhMucIdHienTai > 0) idCanLoai.Add(danhMucIdHienTai);

        return TaoDanhSachDanhMucHienThi(tatCa)
            .Where(item => !idCanLoai.Contains(item.DanhMuc.DanhMucId))
            .Select(item => new CourseCategoryOptionViewModel
            {
                DanhMucId = item.DanhMuc.DanhMucId,
                Label = $"{string.Concat(Enumerable.Repeat("-- ", item.CapDo))}{item.DanhMuc.TenDanhMuc}",
                IsInactive = item.DanhMuc.TrangThai == 0
            })
            .ToList();
    }

    private static HashSet<int> LayTatCaDanhMucConIds(List<DanhMucKhoaHoc> tatCa, int danhMucGocId)
    {
        var ketQua = new HashSet<int>();
        var byParent = tatCa
            .GroupBy(dm => dm.ParentId ?? 0)
            .ToDictionary(group => group.Key, group => group.Select(dm => dm.DanhMucId).ToList());

        void Duyet(int parentId)
        {
            if (!byParent.TryGetValue(parentId, out var childIds)) return;
            foreach (var childId in childIds)
            {
                if (!ketQua.Add(childId)) continue;
                Duyet(childId);
            }
        }

        Duyet(danhMucGocId);
        return ketQua;
    }
}
