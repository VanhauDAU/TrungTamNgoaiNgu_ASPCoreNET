// =============================================================================
// NHÓM 5: NỘI DUNG & TƯƠNG TÁC
// Gồm: Bài viết, Danh mục bài viết, Tags, Liên hệ, Thông báo
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: danhmucbaiviet — Danh mục phân loại bài viết
// ---------------------------------------------------------------------------

[Table("thongbaonguoidung")]
public class ThongBaoNguoiDung
{
    [Key]
    [Column("thongBaoNguoiDungId")]
    public long ThongBaoNguoiDungId { get; set; }

    [Column("thongBaoId")]
    public long? ThongBaoId { get; set; }

    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("daDoc")]
    [Display(Name = "Đã đọc")]
    public bool DaDoc { get; set; }

    [Column("ngayDoc")]
    public DateTime? NgayDoc { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(ThongBaoId))]
    public ThongBao? ThongBao { get; set; }

    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: settings — Cài đặt hệ thống (tên trung tâm, logo, mạng xã hội...)
// ---------------------------------------------------------------------------
