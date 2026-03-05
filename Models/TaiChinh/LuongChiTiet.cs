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

[Table("luongchitiet")]
public class LuongChiTiet
{
    [Key]
    [Column("luongChiTietId")]
    public int LuongChiTietId { get; set; }

    [Column("luongId")]
    public int? LuongId { get; set; }

    [Column("lopHocId")]
    public int? LopHocId { get; set; }

    [Column("soBuoiDay")]
    [Display(Name = "Số buổi dạy")]
    public int? SoBuoiDay { get; set; }

    [Column("donGiaMotBuoi")]
    [Display(Name = "Đơn giá / buổi")]
    public decimal? DonGiaMotBuoi { get; set; }

    [Column("tongTien")]
    [Display(Name = "Thành tiền")]
    public decimal? TongTien { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(LuongId))]
    public Luong? Luong { get; set; }

    [ForeignKey(nameof(LopHocId))]
    public LopHoc? LopHoc { get; set; }
}
