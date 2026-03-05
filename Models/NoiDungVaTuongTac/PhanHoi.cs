using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

[Table("phanhoi")]
public class PhanHoi
{
    [Key]
    [Column("phanHoiId")]
    public int PhanHoiId { get; set; }

    [Column("tieuDe")]
    [MaxLength(255)]
    public string? TieuDe { get; set; }

    [Column("noiDung")]
    public string? NoiDung { get; set; }

    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("danhGia")]
    public byte? DanhGia { get; set; }

    [Column("buoiHocId")]
    public int? BuoiHocId { get; set; }

    [Column("trangThai")]
    public byte? TrangThai { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [ForeignKey(nameof(BuoiHocId))]
    public BuoiHoc? BuoiHoc { get; set; }
}
