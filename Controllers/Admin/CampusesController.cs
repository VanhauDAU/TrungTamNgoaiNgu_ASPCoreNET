using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Controllers.Admin;

public class CampusesController(ICampusService campusService, IHttpClientFactory httpClientFactory) : Controller
{
    public async Task<IActionResult> Index(string? tuKhoa, int? tinhThanhId, int? trangThai)
    {
        var campuses = await campusService.LayDanhSachAsync(tuKhoa, tinhThanhId, trangThai);
        var items = new List<CampusIndexItemViewModel>();

        foreach (var campus in campuses)
        {
            var staff = await campusService.LayNhanSuTheoCoSoAsync(campus.CoSoId);
            items.Add(new CampusIndexItemViewModel
            {
                CoSo = campus,
                SoPhongHoc = campus.PhongHocs.Count,
                SoLopHoc = campus.LopHocs.Count,
                SoNhanSu = staff.Count,
                SoGiaoVien = staff.Count(x => x.Role == 1)
            });
        }

        return View("~/Views/Admin/Campuses/Index.cshtml", new CampusIndexViewModel
        {
            Items = items,
            TinhThanhs = await campusService.LayTinhThanhAsync(),
            ThongKe = await campusService.LayThongKeAsync(),
            TuKhoa = tuKhoa,
            TinhThanhId = tinhThanhId,
            TrangThai = trangThai
        });
    }

    public async Task<IActionResult> Create()
    {
        return View("~/Views/Admin/Campuses/Form.cshtml", await TaoFormViewModelAsync(new CoSoDaoTao()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CampusFormViewModel model)
    {
        ModelState.Remove("Form.MaCoSo");
        ModelState.Remove("Form.Slug");

        if (!ModelState.IsValid)
            return View("~/Views/Admin/Campuses/Form.cshtml", await TaoFormViewModelAsync(model.Form));

        var ketQua = await campusService.ThemAsync(model.Form, LayNguoiThucHien());
        if (!ketQua.ThanhCong)
        {
            ModelState.AddModelError(string.Empty, ketQua.ThongBao);
            return View("~/Views/Admin/Campuses/Form.cshtml", await TaoFormViewModelAsync(model.Form));
        }

        TempData["ThanhCong"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Detail), new { id = ketQua.DuLieu });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var campus = await campusService.LayTheoIdAsync(id);
        if (campus == null) return NotFound();

        return View("~/Views/Admin/Campuses/Form.cshtml", await TaoFormViewModelAsync(campus));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CampusFormViewModel model)
    {
        ModelState.Remove("Form.MaCoSo");
        ModelState.Remove("Form.Slug");

        if (!ModelState.IsValid)
            return View("~/Views/Admin/Campuses/Form.cshtml", await TaoFormViewModelAsync(model.Form));

        var ketQua = await campusService.CapNhatAsync(model.Form, LayNguoiThucHien());
        if (!ketQua.ThanhCong)
        {
            ModelState.AddModelError(string.Empty, ketQua.ThongBao);
            return View("~/Views/Admin/Campuses/Form.cshtml", await TaoFormViewModelAsync(model.Form));
        }

        TempData["ThanhCong"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Detail), new { id = model.Form.CoSoId });
    }

    public async Task<IActionResult> Detail(int id, string? tab = null, int? roomId = null)
    {
        var vm = await TaoDetailViewModelAsync(id, tab, roomId);
        if (vm == null) return NotFound();
        return View("~/Views/Admin/Campuses/Detail.cshtml", vm);
    }

    [HttpGet]
    public async Task<IActionResult> PhuongXaSuggestions(int? tinhThanhId, string? baoGomPhuongXa)
    {
        var phuongXaNoiBo = await campusService.LayPhuongXaNoiBoTheoTinhAsync(tinhThanhId, baoGomPhuongXa);
        if (!tinhThanhId.HasValue || tinhThanhId <= 0)
            return Json(phuongXaNoiBo.Select(name => new { name }));

        var tinhThanh = await campusService.LayTinhThanhTheoIdAsync(tinhThanhId.Value);
        if (tinhThanh?.MaAPI is not int maApi || maApi <= 0)
            return Json(phuongXaNoiBo.Select(name => new { name }));

        try
        {
            var client = httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(8);

            using var response = await client.GetAsync($"https://provinces.open-api.vn/api/p/{maApi}?depth=3");
            if (!response.IsSuccessStatusCode)
                return Json(phuongXaNoiBo.Select(name => new { name }));

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await JsonDocument.ParseAsync(stream);

            var ketQua = new List<string>(phuongXaNoiBo);
            if (document.RootElement.TryGetProperty("districts", out var districts))
            {
                foreach (var district in districts.EnumerateArray())
                {
                    if (!district.TryGetProperty("wards", out var wards))
                        continue;

                    foreach (var ward in wards.EnumerateArray())
                    {
                        if (!ward.TryGetProperty("name", out var nameElement))
                            continue;

                        var wardName = nameElement.GetString()?.Trim();
                        if (!string.IsNullOrWhiteSpace(wardName))
                            ketQua.Add(wardName);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(baoGomPhuongXa))
                ketQua.Add(baoGomPhuongXa.Trim());

            ketQua = ketQua
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(name => name)
                .ToList();

            return Json(ketQua.Select(name => new { name }));
        }
        catch
        {
            return Json(phuongXaNoiBo.Select(name => new { name }));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveRoom(int campusId, PhongHoc form)
    {
        var ketQua = await campusService.LuuPhongTheoCoSoAsync(campusId, form, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;

        if (ketQua.ThanhCong)
            return RedirectToAction(nameof(Detail), new { id = campusId, tab = "rooms" });

        var vm = await TaoDetailViewModelAsync(campusId, "rooms", form.PhongHocId > 0 ? form.PhongHocId : null, form);
        if (vm == null) return NotFound();
        ModelState.AddModelError(string.Empty, ketQua.ThongBao);
        return View("~/Views/Admin/Campuses/Detail.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRoom(int campusId, int id)
    {
        var ketQua = await campusService.XoaPhongAsync(campusId, id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Detail), new { id = campusId, tab = "rooms" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var ketQua = await campusService.XoaAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(Index));
    }

    private async Task<CampusFormViewModel> TaoFormViewModelAsync(CoSoDaoTao form)
    {
        return new CampusFormViewModel
        {
            Form = form,
            TinhThanhs = await campusService.LayTinhThanhAsync()
        };
    }

    private async Task<CampusDetailViewModel?> TaoDetailViewModelAsync(int id, string? tab, int? roomId = null, PhongHoc? roomDraft = null)
    {
        var campus = await campusService.LayTheoIdAsync(id);
        if (campus == null) return null;

        tab = NormalizeTab(tab);

        PhongHoc roomForm = roomDraft ?? new PhongHoc();
        if (roomDraft == null && roomId.HasValue)
        {
            roomForm = await campusService.LayPhongTheoIdAsync(roomId.Value) ?? new PhongHoc();
        }

        if (roomForm.CoSoId.HasValue && roomForm.CoSoId != id)
            roomForm = new PhongHoc();

        roomForm.CoSoId = id;

        return new CampusDetailViewModel
        {
            CoSo = campus,
            TongQuan = await campusService.LayTongQuanChiTietAsync(id),
            PhongHocs = await campusService.LayPhongTheoCoSoAsync(id),
            NhanSus = await campusService.LayNhanSuTheoCoSoAsync(id),
            LopHocs = await campusService.LayLopTheoCoSoAsync(id),
            RoomForm = new CampusRoomFormViewModel { Form = roomForm },
            ActiveTab = tab
        };
    }

    private static string NormalizeTab(string? tab)
    {
        return tab?.ToLowerInvariant() switch
        {
            "rooms" => "rooms",
            "staff" => "staff",
            "classes" => "classes",
            _ => "overview"
        };
    }

    private string LayNguoiThucHien()
        => User?.Identity?.IsAuthenticated == true && !string.IsNullOrWhiteSpace(User.Identity.Name)
            ? User.Identity.Name!
            : "Quản trị viên";
}
