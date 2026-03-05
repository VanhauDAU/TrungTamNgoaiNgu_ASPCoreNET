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

[Table("tags")]
public class Tag
{
    [Key]
    [Column("tagId")]
    public long TagId { get; set; }

    [Required]
    [Column("tenTag")]
    [MaxLength(50)]
    [Display(Name = "Tên tag")]
    public string TenTag { get; set; } = string.Empty;

    [Required]
    [Column("slug")]
    [MaxLength(50)]
    public string Slug { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public ICollection<BaiViet> BaiViets { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: baiviet — Bài viết / Blog của trung tâm
// TrangThai: 0=Ẩn | 1=Công khai
// ---------------------------------------------------------------------------
