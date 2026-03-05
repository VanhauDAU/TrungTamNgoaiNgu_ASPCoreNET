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

[Table("danhmucbaiviet")]
public class DanhMucBaiViet
{
    [Key]
    [Column("danhMucId")]
    public int DanhMucId { get; set; }

    [Required]
    [Column("tenDanhMuc")]
    [MaxLength(100)]
    [Display(Name = "Tên danh mục")]
    public string TenDanhMuc { get; set; } = string.Empty;

    [Required]
    [Column("slug")]
    [MaxLength(100)]
    public string Slug { get; set; } = string.Empty;

    [Column("moTa")]
    public string? MoTa { get; set; }

    [Column("trangThai")]
    public byte TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public ICollection<BaiViet> BaiViets { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: tags — Các nhãn bài viết (IELTS, TOEIC, Speaking...)
// ---------------------------------------------------------------------------
