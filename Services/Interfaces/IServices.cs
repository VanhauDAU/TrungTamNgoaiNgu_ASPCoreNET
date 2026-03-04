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

public class KhoaHocQuanLyThongKe
{
    public int TongKhoaHoc { get; set; }
    public int DangHoatDong { get; set; }
    public int TamNgung { get; set; }
    public int DaXoaMem { get; set; }
    public int LopDangMo { get; set; }
    public int LopSapKhaiGiang { get; set; }
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
// KHOA HOC SERVICE — Quản lý khóa học
// ---------------------------------------------------------------------------
public interface IKhoaHocService
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
    Task<int> ThemAsync(KhoaHoc khoaHoc, string? nguoiThucHien = null);

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
    Task<List<DanhMucKhoaHoc>> LayDanhMucAsync();

    Task<List<DanhMucKhoaHoc>> LayDanhSachDanhMucAsync(string? tuKhoa = null);

    Task<DanhMucKhoaHoc?> LayDanhMucTheoIdAsync(int id);

    Task<int> ThemDanhMucAsync(DanhMucKhoaHoc danhMuc);

    Task<bool> CapNhatDanhMucAsync(DanhMucKhoaHoc danhMuc);

    Task<ServiceResult> XoaMemDanhMucAsync(int id, string? nguoiThucHien = null);

    // Thùng rác danh mục
    Task<List<DanhMucKhoaHoc>> LayThuRacDanhMucAsync();

    // Khôi phục danh mục
    Task<ServiceResult> KhoiPhucDanhMucAsync(int id, string? nguoiThucHien = null);
}

// ---------------------------------------------------------------------------
// LOP HOC SERVICE — Quản lý lớp học
// ---------------------------------------------------------------------------
public interface ILopHocService
{
    Task<List<LopHoc>> LayDanhSachAsync(int? khoaHocId = null, int? coSoId = null, byte? trangThai = null);
    Task<LopHoc?> LayTheoIdAsync(int id);
    Task<int> ThemAsync(LopHoc lopHoc);
    Task<bool> CapNhatAsync(LopHoc lopHoc);
    Task<bool> DoiTrangThaiAsync(int id, byte trangThai);

    // Lấy danh sách học viên trong lớp
    Task<List<DangKyLopHoc>> LayHocVienTrongLopAsync(int lopHocId);
}

// ---------------------------------------------------------------------------
// HOC VIEN SERVICE — Quản lý học viên
// ---------------------------------------------------------------------------
public interface IHocVienService
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
// TAI CHINH SERVICE — Quản lý hóa đơn và phiếu thu
// ---------------------------------------------------------------------------
public interface ITaiChinhService
{
    Task<List<HoaDon>> LayDanhSachHoaDonAsync(int? trangThai = null, string? tuKhoa = null);
    Task<HoaDon?> LayHoaDonTheoIdAsync(int id);

    // Thu tiền: thêm phiếu thu và cập nhật số tiền đã trả trên hóa đơn
    Task<PhieuThu?> ThuTienAsync(int hoaDonId, decimal soTien, byte phuongThuc, string? ghiChu);

    // Thống kê tài chính theo tháng
    Task<decimal> TinhDoanhThuThangAsync(int nam, int thang);
}

// ---------------------------------------------------------------------------
// TAI KHOAN SERVICE — Đăng nhập, phân quyền
// ---------------------------------------------------------------------------
public interface ITaiKhoanService
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
