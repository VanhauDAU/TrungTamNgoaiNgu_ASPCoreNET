// =============================================================================
// NHÓM 2: KHÓA HỌC & LỚP HỌC
// Gồm: Khóa học, Danh mục, Học phí, Lớp học, Ca học, Phòng học, Buổi học
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: danhmuckhoahoc — Danh mục phân loại khóa học (VD: Tiếng Anh, Tiếng Nhật)
// ---------------------------------------------------------------------------

[Table("cahoc")]
public class CaHoc
{
    [Key]
    [Column("caHocId")]
    public int CaHocId { get; set; }

    [Column("tenCa")]
    [MaxLength(50)]
    [Display(Name = "Tên ca")]
    public string? TenCa { get; set; }

    [Column("gioBatDau")]
    [Display(Name = "Giờ bắt đầu")]
    public TimeOnly? GioBatDau { get; set; }

    [Column("gioKetThuc")]
    [Display(Name = "Giờ kết thúc")]
    public TimeOnly? GioKetThuc { get; set; }

    [Column("trangThai")]
    public byte TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

// ---------------------------------------------------------------------------
// BẢNG: phonghoc — Phòng học tại từng cơ sở
// ---------------------------------------------------------------------------
