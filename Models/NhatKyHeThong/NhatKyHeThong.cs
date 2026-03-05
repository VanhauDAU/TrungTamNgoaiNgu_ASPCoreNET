// =============================================================================
// NHÓM 6: NHẬT KÝ HỆ THỐNG
// Ghi log thao tác quản trị phục vụ audit (ai làm gì, khi nào)
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;


[Table("nhatkyhethong")]
public class NhatKyHeThong
{
    [Key]
    [Column("nhatKyId")]
    public long NhatKyId { get; set; }

    [Required]
    [Column("module")]
    [MaxLength(100)]
    public string Module { get; set; } = "HeThong";

    [Required]
    [Column("hanhDong")]
    [MaxLength(150)]
    public string HanhDong { get; set; } = string.Empty;

    [Column("noiDung")]
    public string? NoiDung { get; set; }

    [Required]
    [Column("nguoiThucHien")]
    [MaxLength(150)]
    public string NguoiThucHien { get; set; } = "Hệ thống";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
