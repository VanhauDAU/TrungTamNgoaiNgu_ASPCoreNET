using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

[Table("danhgiagiaovien")]
public class DanhGiaGiaoVien
{
    [Key]
    [Column("danhGiaId")]
    public int DanhGiaId { get; set; }

    [Column("giaoVienId")]
    public int? GiaoVienId { get; set; }

    [Column("hocVienId")]
    public int? HocVienId { get; set; }

    [Column("lopHocId")]
    public int? LopHocId { get; set; }

    [Column("soSao")]
    public byte? SoSao { get; set; }

    [Column("noiDung")]
    public string? NoiDung { get; set; }

    [Column("ngayDanhGia")]
    public DateOnly? NgayDanhGia { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(GiaoVienId))]
    public TaiKhoan? GiaoVien { get; set; }

    [ForeignKey(nameof(HocVienId))]
    public TaiKhoan? HocVien { get; set; }

    [ForeignKey(nameof(LopHocId))]
    public LopHoc? LopHoc { get; set; }
}
