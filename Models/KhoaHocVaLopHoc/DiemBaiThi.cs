using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

[Table("diembaithi")]
public class DiemBaiThi
{
    [Key]
    [Column("diemThiId")]
    public int DiemThiId { get; set; }

    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("baiThiId")]
    public int? BaiThiId { get; set; }

    [Column("diemSo")]
    public decimal? DiemSo { get; set; }

    [Column("ghiChu")]
    public string? GhiChu { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [ForeignKey(nameof(BaiThiId))]
    public BaiThi? BaiThi { get; set; }
}
