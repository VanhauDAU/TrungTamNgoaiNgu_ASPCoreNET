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

[Table("settings")]
public class Setting
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("key")]
    [MaxLength(191)]
    public string Key { get; set; } = string.Empty;

    [Column("value")]
    public string? Value { get; set; }

    [Required]
    [Column("display_name")]
    [MaxLength(191)]
    [Display(Name = "Tên hiển thị")]
    public string DisplayName { get; set; } = string.Empty;

    // text | textarea | image | color
    [Column("type")]
    [MaxLength(50)]
    [Display(Name = "Kiểu dữ liệu")]
    public string Type { get; set; } = "text";

    // general | social | seo | theme
    [Column("group")]
    [MaxLength(50)]
    [Display(Name = "Nhóm cài đặt")]
    public string Group { get; set; } = "general";

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}
