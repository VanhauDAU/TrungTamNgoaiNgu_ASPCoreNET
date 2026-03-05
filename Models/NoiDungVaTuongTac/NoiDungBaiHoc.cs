using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

[Table("noidungbaihoc")]
public class NoiDungBaiHoc
{
    [Key]
    [Column("noiDungId")]
    public long NoiDungId { get; set; }

    [Column("buoiHocId")]
    public int? BuoiHocId { get; set; }

    [Column("tieuDe")]
    [MaxLength(255)]
    public string? TieuDe { get; set; }

    [Column("noiDung")]
    public string? NoiDung { get; set; }

    [Column("taiLieuId")]
    [MaxLength(50)]
    public string? TaiLieuId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(BuoiHocId))]
    public BuoiHoc? BuoiHoc { get; set; }

    [ForeignKey(nameof(TaiLieuId))]
    public TaiLieu? TaiLieu { get; set; }
}
