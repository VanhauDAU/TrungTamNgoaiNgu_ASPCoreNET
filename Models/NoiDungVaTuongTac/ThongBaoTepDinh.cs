using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

[Table("thongbao_tepdinh")]
public class ThongBaoTepDinh
{
    [Key]
    [Column("tepDinhId")]
    public long TepDinhId { get; set; }

    [Column("thongBaoId")]
    public long ThongBaoId { get; set; }

    [Required]
    [Column("tenFile")]
    [MaxLength(255)]
    public string TenFile { get; set; } = string.Empty;

    [Required]
    [Column("tenFileLuu")]
    [MaxLength(255)]
    public string TenFileLuu { get; set; } = string.Empty;

    [Required]
    [Column("duongDan")]
    [MaxLength(500)]
    public string DuongDan { get; set; } = string.Empty;

    [Column("loaiFile")]
    [MaxLength(100)]
    public string? LoaiFile { get; set; }

    [Column("kichThuoc")]
    public long KichThuoc { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(ThongBaoId))]
    public ThongBao? ThongBao { get; set; }
}
