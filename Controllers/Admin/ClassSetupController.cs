using Microsoft.AspNetCore.Mvc;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Controllers.Admin;

public class ClassSetupController(IClassSetupService classSetupService) : Controller
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

    public async Task<IActionResult> CoSo(int? id = null)
    {
        var vm = await TaoCoSoViewModelAsync(id);
        return View("~/Views/Admin/ClassSetup/CoSo.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CoSo(CoSoDaoTao form)
    {
        ModelState.Remove(nameof(CoSoDaoTao.MaCoSo));
        ModelState.Remove(nameof(CoSoDaoTao.Slug));

        var ketQua = await classSetupService.LuuCoSoAsync(form, LayNguoiThucHien());
        if (ketQua.ThanhCong)
        {
            TempData["ThanhCong"] = ketQua.ThongBao;
            return RedirectToAction(nameof(CoSo));
        }

        ModelState.AddModelError(string.Empty, ketQua.ThongBao);
        var vm = await TaoCoSoViewModelAsync(form);
        return View("~/Views/Admin/ClassSetup/CoSo.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCoSo(int id)
    {
        var ketQua = await classSetupService.XoaCoSoAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(CoSo));
    }

    public async Task<IActionResult> PhongHoc(int? id = null)
    {
        var vm = await TaoPhongHocViewModelAsync(id);
        return View("~/Views/Admin/ClassSetup/PhongHoc.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PhongHoc(PhongHoc form)
    {
        var ketQua = await classSetupService.LuuPhongHocAsync(form, LayNguoiThucHien());
        if (ketQua.ThanhCong)
        {
            TempData["ThanhCong"] = ketQua.ThongBao;
            return RedirectToAction(nameof(PhongHoc));
        }

        ModelState.AddModelError(string.Empty, ketQua.ThongBao);
        var vm = await TaoPhongHocViewModelAsync(form);
        return View("~/Views/Admin/ClassSetup/PhongHoc.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePhongHoc(int id)
    {
        var ketQua = await classSetupService.XoaPhongHocAsync(id, LayNguoiThucHien());
        TempData[ketQua.ThanhCong ? "ThanhCong" : "LoiXay"] = ketQua.ThongBao;
        return RedirectToAction(nameof(PhongHoc));
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

    private async Task<CoSoManagementViewModel> TaoCoSoViewModelAsync(int? id)
    {
        var form = id.HasValue ? await classSetupService.LayCoSoTheoIdAsync(id.Value) ?? new CoSoDaoTao() : new CoSoDaoTao();
        return await TaoCoSoViewModelAsync(form);
    }

    private async Task<CoSoManagementViewModel> TaoCoSoViewModelAsync(CoSoDaoTao form)
    {
        var suDung = await classSetupService.LaySoLieuSuDungAsync();
        return new CoSoManagementViewModel
        {
            Form = form,
            Items = await classSetupService.LayDanhSachCoSoAsync(),
            TinhThanhs = await classSetupService.LayTinhThanhAsync(),
            SuDung = suDung
        };
    }

    private async Task<PhongHocManagementViewModel> TaoPhongHocViewModelAsync(int? id)
    {
        var form = id.HasValue ? await classSetupService.LayPhongHocTheoIdAsync(id.Value) ?? new PhongHoc() : new PhongHoc();
        return await TaoPhongHocViewModelAsync(form);
    }

    private async Task<PhongHocManagementViewModel> TaoPhongHocViewModelAsync(PhongHoc form)
    {
        var suDung = await classSetupService.LaySoLieuSuDungAsync();
        return new PhongHocManagementViewModel
        {
            Form = form,
            Items = await classSetupService.LayDanhSachPhongHocAsync(),
            CoSos = await classSetupService.LayCoSoHoatDongAsync(),
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
