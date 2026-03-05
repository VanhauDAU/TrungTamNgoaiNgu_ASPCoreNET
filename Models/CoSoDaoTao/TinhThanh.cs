// =============================================================================
// NHÓM 4: CƠ SỞ ĐÀO TẠO & ĐỊA BÀN
// Gồm: Cơ sở đào tạo, Tỉnh/Thành phố
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: cosodaotao — Các chi nhánh/cơ sở của trung tâm
// ---------------------------------------------------------------------------

[Table("tinhthanh")]
public class TinhThanh
{
    [Key]
    [Column("tinhThanhId")]
    public int TinhThanhId { get; set; }

    // Mã API từ đơn vị hành chính Việt Nam
    [Column("maAPI")]
    public int? MaAPI { get; set; }

    [Required]
    [Column("tenTinhThanh")]
    [MaxLength(100)]
    [Display(Name = "Tỉnh/Thành phố")]
    public string TenTinhThanh { get; set; } = string.Empty;

    [Column("slug")]
    [MaxLength(100)]
    public string Slug { get; set; } = string.Empty;

    [Column("division_type")]
    [MaxLength(50)]
    public string? DivisionType { get; set; }

    [Column("codename")]
    [MaxLength(100)]
    public string? Codename { get; set; }

    public ICollection<CoSoDaoTao> CoSos { get; set; } = [];
}
