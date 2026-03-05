using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

[Table("baithi")]
public class BaiThi
{
    [Key]
    [Column("baiThiId")]
    public int BaiThiId { get; set; }

    [Column("khoaHocId")]
    public int? KhoaHocId { get; set; }

    [Column("tenBaiThi")]
    [MaxLength(255)]
    public string? TenBaiThi { get; set; }

    [Column("moTa")]
    public string? MoTa { get; set; }

    [Column("ngayThi")]
    public DateOnly? NgayThi { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(KhoaHocId))]
    public KhoaHoc? KhoaHoc { get; set; }

    public ICollection<DiemBaiThi> DiemBaiThis { get; set; } = [];
}
