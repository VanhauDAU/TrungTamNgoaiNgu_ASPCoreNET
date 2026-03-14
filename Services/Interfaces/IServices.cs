// =============================================================================
// CÁC INTERFACE (HỢP ĐỒNG) CHO SERVICES
// =============================================================================
// Interface = "Hợp đồng" — định nghĩa CÁI GÌ service phải làm,
// nhưng không định nghĩa LÀM NHƯ THẾ NÀO.
//
// Lợi ích:
// ✅ Controller không phụ thuộc vào implementation cụ thể
// ✅ Dễ viết Unit Test (có thể Mock interface)
// ✅ Có thể đổi implementation mà không sửa Controller
// =============================================================================

using TrungTamNgoaiNgu.Models;

namespace TrungTamNgoaiNgu.Services.Interfaces;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class ServiceResult
{
    public bool ThanhCong { get; set; }
    public string ThongBao { get; set; } = string.Empty;
}

public class ServiceResult<T> : ServiceResult
{
    public T? DuLieu { get; set; }
}

public class KhoaHocQuanLyThongKe
{
    public int TongKhoaHoc { get; set; }
    public int DangHoatDong { get; set; }
    public int TamNgung { get; set; }
    public int DangVanHanh { get; set; }
    public int ChuaCoHocPhi { get; set; }
    public int DaXoaMem { get; set; }
}

public class DanhMucKhoaHocQuanLyThongKe
{
    public int TongDanhMuc { get; set; }
    public int DanhMucGoc { get; set; }
    public int DanhMucCon { get; set; }
    public int DangHoatDong { get; set; }
    public int TamNgung { get; set; }
}

// ---------------------------------------------------------------------------
// DASHBOARD SERVICE — Thống kê tổng quan
// ---------------------------------------------------------------------------
public interface IDashboardService
{
    Task<DashboardThongKe> LayThongKeAsync();
}

// DTO (Data Transfer Object) — Gói dữ liệu trả về cho Dashboard
public class DashboardThongKe
{
    public int SoHocVien { get; set; }
    public int SoGiaoVien { get; set; }
    public int SoNhanVien { get; set; }
    public int SoKhoaHoc { get; set; }
    public int SoLopHocDangDay { get; set; }
    public int SoDangKyMoi { get; set; }        // Trong tháng này
    public decimal DoanhThuThang { get; set; }   // Tháng hiện tại
    public List<DangKyLopHoc> DangKyMoiNhat { get; set; } = [];
    public List<HoaDon> HoaDonChuaThanhToan { get; set; } = [];
}

// ---------------------------------------------------------------------------
// COURSE SERVICE — Quản lý khóa học
// ---------------------------------------------------------------------------
public interface ICoursesService
{
    // Lấy danh sách (phân trang tại DB + tìm kiếm/lọc)
    Task<PagedResult<KhoaHoc>> LayDanhSachPhanTrangAsync(
        string? tuKhoa = null,
        int? danhMucId = null,
        int? trangThai = null,
        int page = 1,
        int pageSize = 10);

    // Tạo slug chuẩn + chống trùng cho khóa học
    Task<string> TaoSlugKhoaHocAsync(string tenKhoaHoc, int? boQuaKhoaHocId = null);

    // Thống kê mini card cho trang quản lý khóa học
    Task<KhoaHocQuanLyThongKe> LayThongKeQuanLyAsync();

    // Lấy 1 khóa học theo ID
    Task<KhoaHoc?> LayTheoIdAsync(int id);

    // Thêm khóa học mới → trả về ID sau khi tạo
    Task<ServiceResult<int>> ThemAsync(KhoaHoc khoaHoc, string? nguoiThucHien = null);

    // Cập nhật khóa học, có kiểm tra nghiệp vụ
    Task<ServiceResult> CapNhatCoKiemTraAsync(KhoaHoc khoaHoc, string? nguoiThucHien = null);

    // Xóa mềm (soft delete) — chỉ set deleted_at, không xóa thật
    Task<ServiceResult> XoaMemAsync(int id, string? nguoiThucHien = null);

    // Thùng rác — lấy danh sách đã xóa mềm
    Task<List<KhoaHoc>> LayThuRacAsync();

    // Khôi phục khóa học từ thùng rác
    Task<ServiceResult> KhoiPhucAsync(int id, string? nguoiThucHien = null);

    // Bulk actions cho khóa học
    Task<ServiceResult> DoiTrangThaiHangLoatAsync(List<int> ids, byte trangThai, string? nguoiThucHien = null);
    Task<ServiceResult> XoaMemHangLoatAsync(List<int> ids, string? nguoiThucHien = null);
    Task<ServiceResult> KhoiPhucHangLoatAsync(List<int> ids, string? nguoiThucHien = null);

    // Lấy tất cả danh mục để hiển thị dropdown
    Task<List<DanhMucKhoaHoc>> LayDanhMucAsync(int? baoGomDanhMucId = null);

    Task<string> TaoSlugDanhMucAsync(string tenDanhMuc, int? boQuaDanhMucId = null);

    Task<List<DanhMucKhoaHoc>> LayDanhSachDanhMucAsync(string? tuKhoa = null);

    Task<DanhMucKhoaHoc?> LayDanhMucTheoIdAsync(int id);

    Task<DanhMucKhoaHocQuanLyThongKe> LayThongKeDanhMucAsync();

    Task<ServiceResult<int>> ThemDanhMucAsync(DanhMucKhoaHoc danhMuc, string? nguoiThucHien = null);

    Task<ServiceResult> CapNhatDanhMucAsync(DanhMucKhoaHoc danhMuc, string? nguoiThucHien = null);

    Task<ServiceResult> XoaMemDanhMucAsync(int id, string? nguoiThucHien = null);

    // Thùng rác danh mục
    Task<List<DanhMucKhoaHoc>> LayThuRacDanhMucAsync();

    // Khôi phục danh mục
    Task<ServiceResult> KhoiPhucDanhMucAsync(int id, string? nguoiThucHien = null);
}

// ---------------------------------------------------------------------------
// CLASSES SERVICE — Quản lý lớp học
// ---------------------------------------------------------------------------

/// <summary>Thống kê mini-cards trang quản lý lớp học</summary>
public class LopHocQuanLyThongKe
{
    public int TongLopHoc     { get; set; }
    public int DangTuyenSinh  { get; set; }
    public int DangHoc        { get; set; }
    public int DaKetThuc      { get; set; }
    public int DaHuy          { get; set; }
}

public interface IClassesService
{
    // ── Danh sách / phân trang / lọc ──────────────────────────────────────
    Task<PagedResult<LopHoc>> LayDanhSachPhanTrangAsync(
        int? khoaHocId    = null,
        int? coSoId       = null,
        int? trangThai    = null,
        string? tuKhoa    = null,
        int  page         = 1,
        int  pageSize     = 10);

    Task<LopHocQuanLyThongKe> LayThongKeAsync();

    // ── Chi tiết ─────────────────────────────────────────────────────────
    /// <summary>Load kèm KhoaHoc, CaHoc, PhongHoc, CoSo, GiaoVien</summary>
    Task<LopHoc?> LayTheoIdAsync(int id);

    // ── CRUD ──────────────────────────────────────────────────────────────
    Task<string> TaoSlugLopHocAsync(string tenLopHoc, int? boQuaId = null);
    Task<ServiceResult> ThemAsync(LopHoc lopHoc, string? nguoiThucHien = null);
    Task<ServiceResult> CapNhatAsync(LopHoc lopHoc, string? nguoiThucHien = null);

    // ── State-machine ─────────────────────────────────────────────────────
    Task<ServiceResult> ChuyenTrangThaiAsync(int id, byte trangThaiMoi, string? nguoiThucHien = null);

    // ── Soft delete / Trash ───────────────────────────────────────────────
    Task<ServiceResult> XoaMemAsync(int id, string? nguoiThucHien = null);
    Task<List<LopHoc>> LayThuRacAsync();
    Task<ServiceResult> KhoiPhucAsync(int id, string? nguoiThucHien = null);

    // ── Dropdowns (cho form Create/Edit) ─────────────────────────────────
    Task<List<KhoaHoc>>    LayKhoaHocDropdownAsync(int? baoGomKhoaHocId = null);
    Task<List<CaHoc>>      LayCaHocDropdownAsync(int? baoGomCaHocId = null);
    Task<List<PhongHoc>>   LayPhongHocDropdownAsync(int? coSoId = null, int? baoGomPhongHocId = null);
    Task<List<CoSoDaoTao>> LayCoSoDropdownAsync(int? baoGomCoSoId = null);
    Task<List<TaiKhoan>>   LayGiaoVienDropdownAsync(int? baoGomTaiKhoanId = null);
    Task<List<HocPhi>>     LayHocPhiDropdownAsync(int? khoaHocId = null, long? baoGomHocPhiId = null);

    // ── Chi tiết học viên & buổi học ─────────────────────────────────────
    Task<List<DangKyLopHoc>> LayHocVienTrongLopAsync(int lopHocId);
    Task<List<BuoiHoc>>      LayBuoiHocAsync(int lopHocId);

    // ── Dropdown địa chỉ 3 tầng ──────────────────────────────────────────
    Task<List<TinhThanh>>  LayTinhThanhDropdownAsync(int? baoGomTinhThanhId = null);
    Task<List<string>>     LayPhuongXaByTinhAsync(int? tinhThanhId, string? baoGomPhuongXa = null);
    Task<List<CoSoDaoTao>> LayCoSoByTinhAsync(int? tinhThanhId, string? phuongXa = null, int? baoGomCoSoId = null);

    // ── Mã lớp tự sinh ───────────────────────────────────────────────────
    /// <summary>Format: K{maKH}-YYYYMM-NNN (VD: KIELTS-202603-001)</summary>
    Task<string> SinhMaLopHocAsync(int? khoaHocId);

}

// ---------------------------------------------------------------------------
// CLASS SETUP SERVICE — Dữ liệu nền để tạo lớp học
// ---------------------------------------------------------------------------
public class ClassSetupThongKe
{
    public int TongCaHoc { get; set; }
    public int TongHocPhi { get; set; }
    public int KhoaHocChuaCoHocPhi { get; set; }
}

public class ClassSetupUsageSnapshot
{
    public Dictionary<int, int> LopTheoCaHoc { get; set; } = [];
    public Dictionary<int, int> BuoiTheoCaHoc { get; set; } = [];
    public Dictionary<long, int> LopTheoHocPhi { get; set; } = [];
}

public interface IClassSetupService
{
    Task<ClassSetupThongKe> LayThongKeAsync();
    Task<ClassSetupUsageSnapshot> LaySoLieuSuDungAsync();

    Task<List<CaHoc>> LayDanhSachCaHocAsync();
    Task<CaHoc?> LayCaHocTheoIdAsync(int id);
    Task<ServiceResult> LuuCaHocAsync(CaHoc caHoc, string? nguoiThucHien = null);
    Task<ServiceResult> XoaCaHocAsync(int id, string? nguoiThucHien = null);

    Task<List<HocPhi>> LayDanhSachHocPhiAsync();
    Task<HocPhi?> LayHocPhiTheoIdAsync(long id);
    Task<ServiceResult> LuuHocPhiAsync(HocPhi hocPhi, string? nguoiThucHien = null);
    Task<ServiceResult> XoaHocPhiAsync(long id, string? nguoiThucHien = null);

    Task<List<KhoaHoc>> LayKhoaHocHoatDongAsync();
}

// ---------------------------------------------------------------------------
// CAMPUS SERVICE — Quản lý cơ sở đào tạo
// ---------------------------------------------------------------------------
public class CampusQuanLyThongKe
{
    public int TongCoSo { get; set; }
    public int CoSoHoatDong { get; set; }
    public int CoSoTamNgung { get; set; }
    public int TongPhongHoc { get; set; }
    public int CoSoDangVanHanh { get; set; }
    public int CoSoChuaCoPhong { get; set; }
}

public class CampusTongQuanChiTiet
{
    public int TongPhongHoc { get; set; }
    public int PhongHoatDong { get; set; }
    public int TongLopHoc { get; set; }
    public int LopDangVanHanh { get; set; }
    public int TongNhanSu { get; set; }
    public int TongGiaoVien { get; set; }
}

public interface ICampusService
{
    Task<CampusQuanLyThongKe> LayThongKeAsync();
    Task<List<CoSoDaoTao>> LayDanhSachAsync(string? tuKhoa = null, int? tinhThanhId = null, int? trangThai = null);
    Task<CoSoDaoTao?> LayTheoIdAsync(int id);
    Task<TinhThanh?> LayTinhThanhTheoIdAsync(int id);
    Task<ServiceResult<int>> ThemAsync(CoSoDaoTao coSo, string? nguoiThucHien = null);
    Task<ServiceResult> CapNhatAsync(CoSoDaoTao coSo, string? nguoiThucHien = null);
    Task<ServiceResult> XoaAsync(int id, string? nguoiThucHien = null);

    Task<CampusTongQuanChiTiet> LayTongQuanChiTietAsync(int coSoId);
    Task<List<PhongHoc>> LayPhongTheoCoSoAsync(int coSoId);
    Task<PhongHoc?> LayPhongTheoIdAsync(int id);
    Task<ServiceResult> LuuPhongTheoCoSoAsync(int coSoId, PhongHoc phongHoc, string? nguoiThucHien = null);
    Task<ServiceResult> XoaPhongAsync(int coSoId, int phongHocId, string? nguoiThucHien = null);

    Task<List<TaiKhoan>> LayNhanSuTheoCoSoAsync(int coSoId);
    Task<List<LopHoc>> LayLopTheoCoSoAsync(int coSoId);
    Task<List<TinhThanh>> LayTinhThanhAsync();
    Task<List<string>> LayPhuongXaNoiBoTheoTinhAsync(int? tinhThanhId, string? baoGomPhuongXa = null);
}

// ---------------------------------------------------------------------------
// STUDENTS SERVICE — Quản lý học viên
// ---------------------------------------------------------------------------
public interface IStudentsService
{
    Task<List<TaiKhoan>> LayDanhSachAsync(string? tuKhoa = null);
    Task<TaiKhoan?> LayTheoIdAsync(int id);
    Task<bool> CapNhatHoSoAsync(HoSoNguoiDung hoSo);

    // Đăng ký học viên vào lớp
    Task<DangKyLopHoc?> DangKyLopAsync(int hocVienId, int lopHocId);

    // Lịch sử đăng ký của học viên
    Task<List<DangKyLopHoc>> LayLichSuDangKyAsync(int hocVienId);
}

// ---------------------------------------------------------------------------
// FINANCE SERVICE — Quản lý hóa đơn và phiếu thu
// ---------------------------------------------------------------------------
public interface IFinanceService
{
    Task<List<HoaDon>> LayDanhSachHoaDonAsync(int? trangThai = null, string? tuKhoa = null);
    Task<HoaDon?> LayHoaDonTheoIdAsync(int id);

    // Thu tiền: thêm phiếu thu và cập nhật số tiền đã trả trên hóa đơn
    Task<PhieuThu?> ThuTienAsync(int hoaDonId, decimal soTien, byte phuongThuc, string? ghiChu);

    // Thống kê tài chính theo tháng
    Task<decimal> TinhDoanhThuThangAsync(int nam, int thang);
}

// ---------------------------------------------------------------------------
// AUTH SERVICE — Đăng nhập, phân quyền
// ---------------------------------------------------------------------------
public interface IAuthService
{
    // Kiểm tra đăng nhập → trả về TaiKhoan nếu đúng, null nếu sai
    Task<TaiKhoan?> DangNhapAsync(string taiKhoan, string matKhau);

    // Kiểm tra quyền truy cập của user
    Task<bool> CoQuyenAsync(int taiKhoanId, string tinhNang, string loaiQuyen);

    Task<List<TaiKhoan>> LayDanhSachNhanVienAsync();
    Task<bool> DoiMatKhauAsync(int taiKhoanId, string matKhauMoi);
    Task<bool> DoiTrangThaiAsync(int taiKhoanId, byte trangThai);
}

// ---------------------------------------------------------------------------
// AUDIT LOGS SERVICE — Xem nhật ký thao tác
// ---------------------------------------------------------------------------
public interface IAuditLogsService
{
    Task<PagedResult<NhatKyHeThong>> LayDanhSachPhanTrangAsync(
        string? module = null,
        string? tuKhoa = null,
        int page = 1,
        int pageSize = 20);

    Task<List<string>> LayDanhSachModuleAsync();
    Task<NhatKyHeThong?> LayTheoIdAsync(long id);
}
