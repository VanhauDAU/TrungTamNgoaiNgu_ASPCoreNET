using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Models;

public class ClassSetupDashboardViewModel
{
    public ClassSetupThongKe ThongKe { get; set; } = new();
    public ClassSetupUsageSnapshot SuDung { get; set; } = new();
}

public class CaHocManagementViewModel
{
    public List<CaHoc> Items { get; set; } = [];
    public CaHoc Form { get; set; } = new();
    public ClassSetupUsageSnapshot SuDung { get; set; } = new();
    public bool IsEditing => Form.CaHocId > 0;
}

public class HocPhiManagementViewModel
{
    public List<HocPhi> Items { get; set; } = [];
    public List<KhoaHoc> KhoaHocs { get; set; } = [];
    public HocPhi Form { get; set; } = new();
    public ClassSetupUsageSnapshot SuDung { get; set; } = new();
    public bool IsEditing => Form.HocPhiId > 0;
}

public class CoSoManagementViewModel
{
    public List<CoSoDaoTao> Items { get; set; } = [];
    public List<TinhThanh> TinhThanhs { get; set; } = [];
    public CoSoDaoTao Form { get; set; } = new();
    public ClassSetupUsageSnapshot SuDung { get; set; } = new();
    public bool IsEditing => Form.CoSoId > 0;
}

public class PhongHocManagementViewModel
{
    public List<PhongHoc> Items { get; set; } = [];
    public List<CoSoDaoTao> CoSos { get; set; } = [];
    public PhongHoc Form { get; set; } = new();
    public ClassSetupUsageSnapshot SuDung { get; set; } = new();
    public bool IsEditing => Form.PhongHocId > 0;
}
