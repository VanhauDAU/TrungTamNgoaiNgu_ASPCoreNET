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

[Table("lienhe")]
public class LienHe
{
    [Key]
    [Column("lienHeId")]
    public int LienHeId { get; set; }

    [Column("hoTen")]
    [MaxLength(100)]
    [Display(Name = "Họ tên")]
    public string? HoTen { get; set; }

    [Column("email")]
    [MaxLength(100)]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Column("soDienThoai")]
    [MaxLength(15)]
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [Column("tieuDe")]
    [MaxLength(255)]
    [Display(Name = "Tiêu đề")]
    public string? TieuDe { get; set; }

    [Column("noiDung")]
    [Display(Name = "Nội dung")]
    public string? NoiDung { get; set; }

    // 0=Chưa xử lý | 1=Đã xử lý
    [Column("trangThai")]
    public byte? TrangThai { get; set; }

    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

// ---------------------------------------------------------------------------
// BẢNG: thongbao — Thông báo gửi đến học viên / nhân viên
// LoaiGui: 0=Hệ thống | 1=Học tập | 2=Tài chính | 3=Sự kiện | 4=Khẩn cấp
// DoiTuongGui: 0=Tất cả | 1=Theo lớp | 2=Theo khóa học | 3=Cá nhân
// ---------------------------------------------------------------------------
