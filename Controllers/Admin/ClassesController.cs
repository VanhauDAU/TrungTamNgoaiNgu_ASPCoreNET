// =============================================================================
// ADMIN CLASSES CONTROLLER
// =============================================================================
// Quản lý lớp học CRUD + state-machine + soft delete + bulk actions
// URL pattern: /Admin/Classes/{action}
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Controllers.Admin;

public class ClassesController(IClassesService classesService, IHttpClientFactory httpClientFactory) : Controller
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
        ModelState.Remove("MaLopHoc");

        if (!ModelState.IsValid)
        {
            await NapDropdowns(lopHoc);
            return View("~/Views/Admin/Classes/Create.cshtml", lopHoc);
        }

        lopHoc.Slug = await classesService.TaoSlugLopHocAsync(lopHoc.TenLopHoc ?? "lop-hoc");

        var ketQua = await classesService.ThemAsync(lopHoc, LayNguoiThucHien());
        if (!ketQua.ThanhCong)
        {
            ModelState.AddModelError(string.Empty, ketQua.ThongBao);
            await NapDropdowns(lopHoc);
            return View("~/Views/Admin/Classes/Create.cshtml", lopHoc);
        }

        TempData["ThanhCong"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Index));
    }

    // =========================================================================
    // EDIT — Sửa lớp học
    // =========================================================================
    public async Task<IActionResult> Edit(int id)
    {
        var lopHoc = await classesService.LayTheoIdAsync(id);
        if (lopHoc == null) return NotFound();

        await NapDropdowns(lopHoc);
        return View("~/Views/Admin/Classes/Edit.cshtml", lopHoc);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(LopHoc lopHoc)
    {
        ModelState.Remove("Slug");

        if (!ModelState.IsValid)
        {
            await NapDropdowns(lopHoc);
            return View("~/Views/Admin/Classes/Edit.cshtml", lopHoc);
        }

        lopHoc.Slug = await classesService.TaoSlugLopHocAsync(
            lopHoc.TenLopHoc ?? "lop-hoc", lopHoc.LopHocId);

        var ketQua = await classesService.CapNhatAsync(lopHoc, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;

        if (!ketQua.ThanhCong)
        {
            await NapDropdowns(lopHoc);
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
        return Json(phongHocs.Select(p => new
        {
            id = p.PhongHocId,
            name = p.TenPhong,
            sucChua = p.SucChua
        }));
    }

    /// <summary>GET /Admin/Classes/hocphi-by-khoahoc?khoaHocId=1</summary>
    [HttpGet]
    public async Task<IActionResult> HocPhiByKhoaHoc(int? khoaHocId)
    {
        if (!khoaHocId.HasValue || khoaHocId <= 0)
            return Json(Array.Empty<object>());

        var hocPhis = await classesService.LayHocPhiDropdownAsync(khoaHocId);
        return Json(hocPhis.Select(h => new
        {
            id = h.HocPhiId,
            name = $"{h.SoBuoi} buổi - {h.DonGia?.ToString("N0") ?? "?"}đ/buổi",
            soBuoi = h.SoBuoi,
            donGia = h.DonGia,
            tongHocPhi = h.TongHocPhi,
            tongHocPhiText = h.TongHocPhi.ToString("N0")
        }));
    }
    /// <summary>GET /Admin/Classes/CoSoByTinh?tinhThanhId=1 — AJAX: cơ sở theo tỉnh</summary>
    [HttpGet]
    public async Task<IActionResult> CoSoByTinh(int? tinhThanhId, string? phuongXa)
    {
        var coSos = await classesService.LayCoSoByTinhAsync(tinhThanhId, phuongXa);
        return Json(coSos.Select(c => new
        {
            id = c.CoSoId,
            name = c.TenCoSo,
            diaChi = c.DiaChi ?? "",
            phuongXa = c.TenPhuongXa ?? ""
        }));
    }

    /// <summary>GET /Admin/Classes/PhuongXaByTinh?maApi=79 — AJAX: phường/xã theo mã tỉnh API</summary>
    [HttpGet]
    public async Task<IActionResult> PhuongXaByTinh(int? maApi)
    {
        if (!maApi.HasValue || maApi <= 0)
            return Json(Array.Empty<object>());

        var phuongXa = await LayPhuongXaTuOpenApiAsync(maApi.Value);
        return Json(phuongXa.Select(px => new
        {
            name = px.Name,
            district = px.District
        }));

    }

    /// <summary>GET /Admin/Classes/SinhMaLop?khoaHocId=1 — AJAX: sinh mã lớp tự động</summary>
    [HttpGet]
    public async Task<IActionResult> SinhMaLop(int? khoaHocId)
    {
        var ma = await classesService.SinhMaLopHocAsync(khoaHocId);
        return Json(new { ma });
    }
    // =========================================================================
    // PRIVATE HELPERS
    // =========================================================================

    private async Task NapDropdowns(LopHoc? lopHoc = null)
    {
        var khoaHocId = lopHoc?.KhoaHocId;
        var coSoId = lopHoc?.CoSoId;
        var caHocId = lopHoc?.CaHocId > 0 ? (int?)lopHoc.CaHocId : null;
        var phongHocId = lopHoc?.PhongHocId;
        var hocPhiId = lopHoc?.HocPhiId;
        var taiKhoanId = lopHoc?.TaiKhoanId;

        ViewBag.TinhThanhs = await classesService.LayTinhThanhDropdownAsync();

        ViewBag.KhoaHocs   = await classesService.LayKhoaHocDropdownAsync(khoaHocId);
        ViewBag.CaHocs     = await classesService.LayCaHocDropdownAsync(caHocId);
        ViewBag.CoSos      = await classesService.LayCoSoDropdownAsync(coSoId);
        ViewBag.PhongHocs  = await classesService.LayPhongHocDropdownAsync(coSoId, phongHocId);
        ViewBag.GiaoViens  = await classesService.LayGiaoVienDropdownAsync(taiKhoanId);
        ViewBag.HocPhis    = await classesService.LayHocPhiDropdownAsync(khoaHocId, hocPhiId);
    }

    private string LayNguoiThucHien()
        => User?.Identity?.IsAuthenticated == true && !string.IsNullOrWhiteSpace(User.Identity.Name)
            ? User.Identity.Name!
            : "Quản trị viên";

    private async Task<List<PhuongXaDto>> LayPhuongXaTuOpenApiAsync(int maApi)
    {
        var client = httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(10);
        client.DefaultRequestHeaders.UserAgent.ParseAdd("ASPCORETrungTamNN/1.0");

        var urls = new[]
        {
            $"https://provinces.open-api.vn/api/v2/p/{maApi}?depth=2",
            $"https://provinces.open-api.vn/api/p/{maApi}?depth=3"
        };

        foreach (var url in urls)
        {
            try
            {
                using var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode) continue;

                var payload = await response.Content.ReadAsStringAsync();
                var wards = ParsePhuongXa(payload);
                if (wards.Count > 0) return wards;
            }
            catch
            {
                // fallback sang endpoint kế tiếp
            }
        }

        return [];
    }

    private static List<PhuongXaDto> ParsePhuongXa(string payload)
    {
        var ketQua = new List<PhuongXaDto>();

        try
        {
            using var doc = JsonDocument.Parse(payload);

            // v2 mới: dữ liệu ward nằm trực tiếp ở root.wards
            if (doc.RootElement.TryGetProperty("wards", out var wardsV2) &&
                wardsV2.ValueKind == JsonValueKind.Array)
            {
                foreach (var ward in wardsV2.EnumerateArray())
                {
                    if (!ward.TryGetProperty("name", out var wardName)) continue;
                    var tenPhuongXa = wardName.GetString()?.Trim();
                    if (string.IsNullOrWhiteSpace(tenPhuongXa)) continue;
                    ketQua.Add(new PhuongXaDto(tenPhuongXa, string.Empty));
                }
                return ketQua.OrderBy(x => x.Name).ToList();
            }

            // v1 cũ: districts[].wards[]
            if (!doc.RootElement.TryGetProperty("districts", out var districts) ||
                districts.ValueKind != JsonValueKind.Array)
            {
                return ketQua;
            }

            foreach (var district in districts.EnumerateArray())
            {
                var tenQuanHuyen = district.TryGetProperty("name", out var districtName)
                    ? districtName.GetString()?.Trim() ?? string.Empty
                    : string.Empty;

                if (!district.TryGetProperty("wards", out var wards) ||
                    wards.ValueKind != JsonValueKind.Array)
                {
                    continue;
                }

                foreach (var ward in wards.EnumerateArray())
                {
                    if (!ward.TryGetProperty("name", out var wardName)) continue;

                    var tenPhuongXa = wardName.GetString()?.Trim();
                    if (string.IsNullOrWhiteSpace(tenPhuongXa)) continue;

                    ketQua.Add(new PhuongXaDto(tenPhuongXa, tenQuanHuyen));
                }
            }
        }
        catch
        {
            // payload không hợp lệ
        }

        return ketQua
            .GroupBy(x => new { x.Name, x.District })
            .Select(g => g.First())
            .OrderBy(x => x.Name)
            .ToList();
    }

    private sealed record PhuongXaDto(string Name, string District);
}
