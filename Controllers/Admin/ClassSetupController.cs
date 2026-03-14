using Microsoft.AspNetCore.Mvc;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Controllers.Admin;

public class ClassSetupController(IClassSetupService classSetupService, ICampusService campusService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var suDung = await classSetupService.LaySoLieuSuDungAsync();
        var vm = new ClassSetupDashboardViewModel
        {
            ThongKe = await classSetupService.LayThongKeAsync(),
            SuDung = suDung
        };

        return View("~/Views/Admin/ClassSetup/Index.cshtml", vm);
    }

    public async Task<IActionResult> CaHoc(int? id = null)
    {
        var vm = await TaoCaHocViewModelAsync(id);
        return View("~/Views/Admin/ClassSetup/CaHoc.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CaHoc(CaHoc form)
    {
        var ketQua = await classSetupService.LuuCaHocAsync(form, LayNguoiThucHien());
        if (ketQua.ThanhCong)
        {
            TempData["ThanhCong"] = ketQua.ThongBao;
            return RedirectToAction(nameof(CaHoc));
        }

        ModelState.AddModelError(string.Empty, ketQua.ThongBao);
        var vm = await TaoCaHocViewModelAsync(form);
        return View("~/Views/Admin/ClassSetup/CaHoc.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCaHoc(int id)
    {
        var ketQua = await classSetupService.XoaCaHocAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(CaHoc));
    }

    public async Task<IActionResult> HocPhi(long? id = null)
    {
        var vm = await TaoHocPhiViewModelAsync(id);
        return View("~/Views/Admin/ClassSetup/HocPhi.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HocPhi(HocPhi form)
    {
        var ketQua = await classSetupService.LuuHocPhiAsync(form, LayNguoiThucHien());
        if (ketQua.ThanhCong)
        {
            TempData["ThanhCong"] = ketQua.ThongBao;
            return RedirectToAction(nameof(HocPhi));
        }

        ModelState.AddModelError(string.Empty, ketQua.ThongBao);
        var vm = await TaoHocPhiViewModelAsync(form);
        return View("~/Views/Admin/ClassSetup/HocPhi.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteHocPhi(long id)
    {
        var ketQua = await classSetupService.XoaHocPhiAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(HocPhi));
    }

    public IActionResult CoSo(int? id = null)
    {
        if (id.HasValue && id.Value > 0)
            return RedirectToAction("Edit", "Campuses", new { id = id.Value });

        return RedirectToAction("Index", "Campuses");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CoSo(CoSoDaoTao form)
    {
        TempData["LoiXay"] = "Màn hình cơ sở đã chuyển sang module `Cơ sở đào tạo` mới.";
        return RedirectToAction("Index", "Campuses");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteCoSo(int id)
    {
        TempData["LoiXay"] = "Hành động này đã chuyển sang module `Cơ sở đào tạo` mới.";
        return RedirectToAction("Index", "Campuses");
    }

    public async Task<IActionResult> PhongHoc(int? id = null)
    {
        if (id.HasValue && id.Value > 0)
        {
            var room = await campusService.LayPhongTheoIdAsync(id.Value);
            if (room?.CoSoId.HasValue == true)
                return RedirectToAction("Detail", "Campuses", new { id = room.CoSoId.Value, tab = "rooms", roomId = id.Value });
        }

        return RedirectToAction("Index", "Campuses");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PhongHoc(PhongHoc form)
    {
        TempData["LoiXay"] = "Màn hình phòng học đã chuyển vào chi tiết cơ sở.";
        return RedirectToAction("Index", "Campuses");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePhongHoc(int id)
    {
        TempData["LoiXay"] = "Hành động này đã chuyển vào chi tiết cơ sở.";
        return RedirectToAction("Index", "Campuses");
    }

    private async Task<CaHocManagementViewModel> TaoCaHocViewModelAsync(int? id)
    {
        var form = id.HasValue ? await classSetupService.LayCaHocTheoIdAsync(id.Value) ?? new CaHoc() : new CaHoc();
        return await TaoCaHocViewModelAsync(form);
    }

    private async Task<CaHocManagementViewModel> TaoCaHocViewModelAsync(CaHoc form)
    {
        var suDung = await classSetupService.LaySoLieuSuDungAsync();
        return new CaHocManagementViewModel
        {
            Form = form,
            Items = await classSetupService.LayDanhSachCaHocAsync(),
            SuDung = suDung
        };
    }

    private async Task<HocPhiManagementViewModel> TaoHocPhiViewModelAsync(long? id)
    {
        var form = id.HasValue ? await classSetupService.LayHocPhiTheoIdAsync(id.Value) ?? new HocPhi() : new HocPhi();
        return await TaoHocPhiViewModelAsync(form);
    }

    private async Task<HocPhiManagementViewModel> TaoHocPhiViewModelAsync(HocPhi form)
    {
        var suDung = await classSetupService.LaySoLieuSuDungAsync();
        return new HocPhiManagementViewModel
        {
            Form = form,
            Items = await classSetupService.LayDanhSachHocPhiAsync(),
            KhoaHocs = await classSetupService.LayKhoaHocHoatDongAsync(),
            SuDung = suDung
        };
    }

    private string LayNguoiThucHien()
    {
        if (User?.Identity?.IsAuthenticated == true && !string.IsNullOrWhiteSpace(User.Identity.Name))
            return User.Identity.Name!;
        return "Quản trị viên";
    }
}
