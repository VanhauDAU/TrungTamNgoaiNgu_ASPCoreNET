using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Models;

public class CourseCategoryOptionViewModel
{
    public int DanhMucId { get; set; }
    public string Label { get; set; } = string.Empty;
    public bool IsInactive { get; set; }
}

public class CourseCategoryListItemViewModel
{
    public DanhMucKhoaHoc DanhMuc { get; set; } = new();
    public int CapDo { get; set; }
    public string TenHienThi { get; set; } = string.Empty;
    public string DuongDanCha { get; set; } = string.Empty;
    public int SoDanhMucCon { get; set; }
    public int SoKhoaHocTong { get; set; }
    public int SoKhoaHocDangHoatDong { get; set; }
}

public class CourseCategoryIndexViewModel
{
    public List<CourseCategoryListItemViewModel> Items { get; set; } = [];
    public DanhMucKhoaHocQuanLyThongKe ThongKe { get; set; } = new();
}

public class CourseCategoryFormViewModel
{
    public DanhMucKhoaHoc Form { get; set; } = new();
    public List<CourseCategoryOptionViewModel> ParentOptions { get; set; } = [];
    public string SlugPreview { get; set; } = string.Empty;
    public int SoDanhMucCon { get; set; }
    public int SoKhoaHocTong { get; set; }
    public int SoKhoaHocDangHoatDong { get; set; }
    public bool IsEditing => Form.DanhMucId > 0;
}
