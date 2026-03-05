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

[Table("luong")]
public class Luong
{
    [Key]
    [Column("luongId")]
    public int LuongId { get; set; }

    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("thangLuong")]
    [MaxLength(10)]
    [Display(Name = "Tháng lương")]
    public string? ThangLuong { get; set; } // VD: "2026-03"

    [Column("tongLuongDay")]
    [Display(Name = "Tổng lương đứng lớp")]
    public decimal? TongLuongDay { get; set; }

    [Column("thuong")]
    [Display(Name = "Thưởng")]
    public decimal Thuong { get; set; }

    [Column("phat")]
    [Display(Name = "Phạt / Khấu trừ")]
    public decimal Phat { get; set; }

    [Column("phuCap")]
    [Display(Name = "Phụ cấp")]
    public decimal PhuCap { get; set; }

    [Column("tongTienThucLanh")]
    [Display(Name = "Thực lĩnh")]
    public decimal? TongTienThucLanh { get; set; }

    [Column("tongBuoiDay")]
    [Display(Name = "Tổng buổi dạy")]
    public int? TongBuoiDay { get; set; }

    [Column("ngayThanhToan")]
    [Display(Name = "Ngày thanh toán")]
    public DateOnly? NgayThanhToan { get; set; }

    [Column("trangThai")]
    public byte? TrangThai { get; set; }

    [Column("ghiChu")]
    public string? GhiChu { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }
    public ICollection<LuongChiTiet> ChiTiets { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: luongchitiet — Chi tiết từng lớp giáo viên đã dạy trong tháng
// ---------------------------------------------------------------------------
