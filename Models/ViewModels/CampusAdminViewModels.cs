using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Models;

public class CampusIndexItemViewModel
{
    public CoSoDaoTao CoSo { get; set; } = new();
    public int SoPhongHoc { get; set; }
    public int SoLopHoc { get; set; }
    public int SoNhanSu { get; set; }
    public int SoGiaoVien { get; set; }
}

public class CampusIndexViewModel
{
    public List<CampusIndexItemViewModel> Items { get; set; } = [];
    public List<TinhThanh> TinhThanhs { get; set; } = [];
    public CampusQuanLyThongKe ThongKe { get; set; } = new();
    public string? TuKhoa { get; set; }
    public int? TinhThanhId { get; set; }
    public int? TrangThai { get; set; }
}

public class CampusFormViewModel
{
    public CoSoDaoTao Form { get; set; } = new();
    public List<TinhThanh> TinhThanhs { get; set; } = [];
    public bool IsEditing => Form.CoSoId > 0;
}

public class CampusRoomFormViewModel
{
    public PhongHoc Form { get; set; } = new();
    public bool IsEditing => Form.PhongHocId > 0;
}

public class CampusDetailViewModel
{
    public CoSoDaoTao CoSo { get; set; } = new();
    public CampusTongQuanChiTiet TongQuan { get; set; } = new();
    public List<PhongHoc> PhongHocs { get; set; } = [];
    public List<TaiKhoan> NhanSus { get; set; } = [];
    public List<LopHoc> LopHocs { get; set; } = [];
    public CampusRoomFormViewModel RoomForm { get; set; } = new();
    public string ActiveTab { get; set; } = "overview";
}
