using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

[Table("lienhe_lichsu")]
public class LienHeLichSu
{
    [Key]
    [Column("lichSuId")]
    public long LichSuId { get; set; }

    [Column("lienHeId")]
    public int LienHeId { get; set; }

    [Required]
    [Column("hanhDong")]
    [MaxLength(100)]
    public string HanhDong { get; set; } = string.Empty;

    [Column("noiDung")]
    public string? NoiDung { get; set; }

    [Column("giaTriCu")]
    [MaxLength(200)]
    public string? GiaTriCu { get; set; }

    [Column("giaTriMoi")]
    [MaxLength(200)]
    public string? GiaTriMoi { get; set; }

    [Column("nguoiThucHienId")]
    public int? NguoiThucHienId { get; set; }

    [Column("tenNguoiThucHien")]
    [MaxLength(200)]
    public string? TenNguoiThucHien { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(LienHeId))]
    public LienHe? LienHe { get; set; }
}
