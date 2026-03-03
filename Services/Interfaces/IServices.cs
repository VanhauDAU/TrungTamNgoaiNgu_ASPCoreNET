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
    // Lấy danh sách (có phân trang và tìm kiếm)
    Task<List<KhoaHoc>> LayDanhSachAsync(string? tuKhoa = null, int? danhMucId = null);

    // Lấy 1 khóa học theo ID
    Task<KhoaHoc?> LayTheoIdAsync(int id);

    // Thêm khóa học mới → trả về ID sau khi tạo
    Task<int> ThemAsync(KhoaHoc khoaHoc);

    // Cập nhật khóa học → trả về true nếu thành công
    Task<bool> CapNhatAsync(KhoaHoc khoaHoc);

    // Xóa mềm (soft delete) — chỉ set deleted_at, không xóa thật
    Task<bool> XoaMemAsync(int id);

    // Lấy tất cả danh mục để hiển thị dropdown
    Task<List<DanhMucKhoaHoc>> LayDanhMucAsync();

    Task<List<DanhMucKhoaHoc>> LayDanhSachDanhMucAsync(string? tuKhoa = null);

    Task<DanhMucKhoaHoc?> LayDanhMucTheoIdAsync(int id);

    Task<int> ThemDanhMucAsync(DanhMucKhoaHoc danhMuc);

    Task<bool> CapNhatDanhMucAsync(DanhMucKhoaHoc danhMuc);

    Task<bool> XoaMemDanhMucAsync(int id);
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
