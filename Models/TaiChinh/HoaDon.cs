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

[Table("hoadon")]
public class HoaDon
{
    [Key]
    [Column("hoaDonId")]
    public int HoaDonId { get; set; }

    [Column("maHoaDon")]
    [MaxLength(20)]
    [Display(Name = "Mã hóa đơn")]
    public string? MaHoaDon { get; set; }

    [Column("ngayLap")]
    [Display(Name = "Ngày lập")]
    public DateOnly? NgayLap { get; set; }

    [Column("ngayHetHan")]
    [Display(Name = "Hạn thanh toán")]
    public DateOnly? NgayHetHan { get; set; }

    [Column("tongTien")]
    [Display(Name = "Tổng tiền")]
    public decimal? TongTien { get; set; }

    [Column("giamGia")]
    [Display(Name = "Giảm giá")]
    public decimal GiamGia { get; set; }

    [Column("thue")]
    [Display(Name = "Thuế (%)")]
    public decimal Thue { get; set; }

    [Column("tongTienSauThue")]
    [Display(Name = "Tổng sau thuế")]
    public decimal TongTienSauThue { get; set; }

    [Column("daTra")]
    [Display(Name = "Đã thanh toán")]
    public decimal? DaTra { get; set; }

    // Học viên được lập hóa đơn
    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    // Đăng ký lớp học nào
    [Column("dangKyLopHocId")]
    public int? DangKyLopHocId { get; set; }

    // 1=Tiền mặt | 2=Chuyển khoản | 3=VNPay
    [Column("phuongThucThanhToan")]
    [Display(Name = "Phương thức TT")]
    public int? PhuongThucThanhToan { get; set; }

    // 0=Đăng ký mới | 1=Gia hạn | 2=Khác
    [Column("loaiHoaDon")]
    [Display(Name = "Loại hóa đơn")]
    public byte LoaiHoaDon { get; set; }

    [Column("coSoId")]
    public int? CoSoId { get; set; }

    // 0=Chưa thanh toán | 1=Thanh toán một phần | 2=Đã đủ
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public byte? TrangThai { get; set; }

    [Column("ghiChu")]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [ForeignKey(nameof(DangKyLopHocId))]
    public DangKyLopHoc? DangKyLopHoc { get; set; }

    [ForeignKey(nameof(CoSoId))]
    public CoSoDaoTao? CoSo { get; set; }

    public ICollection<PhieuThu> PhieuThus { get; set; } = [];

    // Số tiền còn nợ
    [NotMapped]
    public decimal ConNo => (TongTienSauThue > 0 ? TongTienSauThue : TongTien ?? 0) - (DaTra ?? 0);

    [NotMapped]
    public string TrangThaiText => TrangThai switch {
        0 => "Chưa thanh toán", 1 => "Thanh toán một phần", 2 => "Đã thanh toán đủ",
        _ => "Không xác định"
    };

    [NotMapped]
    public string PhuongThucText => PhuongThucThanhToan switch {
        1 => "Tiền mặt", 2 => "Chuyển khoản", 3 => "VNPay", _ => "Chưa xác định"
    };
}

// ---------------------------------------------------------------------------
// BẢNG: phieuthu — Từng lần thu tiền (1 hóa đơn có thể có nhiều phiếu thu)
// TrangThai: 0=Hủy | 1=Hợp lệ
// ---------------------------------------------------------------------------
