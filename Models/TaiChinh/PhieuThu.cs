// =============================================================================
// NHÓM 3: TÀI CHÍNH
// Gồm: Hóa đơn, Phiếu thu, Lương giáo viên, Chi tiết lương
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: hoadon — Hóa đơn khi học viên đăng ký lớp học
// TrangThai: 0=Chưa thanh toán | 1=Thanh toán một phần | 2=Đã thanh toán đủ
// ---------------------------------------------------------------------------

[Table("phieuthu")]
public class PhieuThu
{
    [Key]
    [Column("phieuThuId")]
    public long PhieuThuId { get; set; }

    [Column("maPhieuThu")]
    [MaxLength(20)]
    [Display(Name = "Mã phiếu thu")]
    public string? MaPhieuThu { get; set; }

    [Column("hoaDonId")]
    public int? HoaDonId { get; set; }

    [Column("soTien")]
    [Display(Name = "Số tiền")]
    public decimal? SoTien { get; set; }

    // 1=Tiền mặt | 2=Chuyển khoản | 3=VNPay
    [Column("phuongThucThanhToan")]
    [Display(Name = "Phương thức TT")]
    public byte PhuongThucThanhToan { get; set; } = 1;

    [Column("ngayThu")]
    [Display(Name = "Ngày thu")]
    public DateOnly? NgayThu { get; set; }

    // Nhân viên thu tiền
    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("ghiChu")]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    // 0=Hủy | 1=Hợp lệ
    [Column("trangThai")]
    public byte TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(HoaDonId))]
    public HoaDon? HoaDon { get; set; }

    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? NguoiThu { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: luong — Bảng lương hàng tháng của giáo viên/nhân viên
// ---------------------------------------------------------------------------
