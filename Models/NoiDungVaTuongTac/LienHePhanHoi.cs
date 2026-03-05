using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

[Table("lienhe_phanhoi")]
public class LienHePhanHoi
{
    [Key]
    [Column("phanHoiId")]
    public long PhanHoiId { get; set; }

    [Column("lienHeId")]
    public int LienHeId { get; set; }

    [Required]
    [Column("noiDung")]
    public string NoiDung { get; set; } = string.Empty;

    [Required]
    [Column("loai")]
    [MaxLength(20)]
    public string Loai { get; set; } = "noi_bo";

    [Column("nguoiGuiId")]
    public int? NguoiGuiId { get; set; }

    [Column("tenNguoiGui")]
    [MaxLength(200)]
    public string? TenNguoiGui { get; set; }

    [Column("daGuiEmail")]
    public bool DaGuiEmail { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(LienHeId))]
    public LienHe? LienHe { get; set; }
}
