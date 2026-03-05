using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

[Table("tailieu")]
public class TaiLieu
{
    [Key]
    [Column("taiLieuId")]
    [MaxLength(50)]
    public string TaiLieuId { get; set; } = Guid.NewGuid().ToString();

    [Column("tenTaiLieu")]
    [MaxLength(255)]
    public string? TenTaiLieu { get; set; }

    [Column("moTa")]
    public string? MoTa { get; set; }

    [Column("loaiTaiLieu")]
    [MaxLength(50)]
    public string? LoaiTaiLieu { get; set; }

    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("duongDan")]
    [MaxLength(255)]
    public string? DuongDan { get; set; }

    [Column("khoaHocId")]
    public int? KhoaHocId { get; set; }

    [Column("buoiHocId")]
    public int? BuoiHocId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [ForeignKey(nameof(KhoaHocId))]
    public KhoaHoc? KhoaHoc { get; set; }

    [ForeignKey(nameof(BuoiHocId))]
    public BuoiHoc? BuoiHoc { get; set; }

    public ICollection<NoiDungBaiHoc> NoiDungBaiHocs { get; set; } = [];
}
