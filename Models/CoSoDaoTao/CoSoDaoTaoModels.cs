// =============================================================================
// NHÓM 4: CƠ SỞ ĐÀO TẠO & ĐỊA BÀN
// Gồm: Cơ sở đào tạo, Tỉnh/Thành phố
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: cosodaotao — Các chi nhánh/cơ sở của trung tâm
// ---------------------------------------------------------------------------
[Table("cosodaotao")]
public class CoSoDaoTao
{
    [Key]
    [Column("coSoId")]
    public int CoSoId { get; set; }

    [Required]
    [Column("maCoSo")]
    [MaxLength(20)]
    [Display(Name = "Mã cơ sở")]
    public string MaCoSo { get; set; } = string.Empty;

    [Required]
    [Column("slug")]
    [MaxLength(150)]
    public string Slug { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên cơ sở không được để trống")]
    [Column("tenCoSo")]
    [MaxLength(255)]
    [Display(Name = "Tên cơ sở")]
    public string TenCoSo { get; set; } = string.Empty;

    [Column("diaChi")]
    [MaxLength(255)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    [Column("soDienThoai")]
    [MaxLength(20)]
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [Column("email")]
    [MaxLength(100)]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    // Link nhúng Google Maps
    [Column("banDoGoogle")]
    [Display(Name = "Bản đồ Google")]
    public string? BanDoGoogle { get; set; }

    [Column("tinhThanhId")]
    public int? TinhThanhId { get; set; }

    [Column("tenPhuongXa")]
    [MaxLength(150)]
    [Display(Name = "Phường/Xã")]
    public string? TenPhuongXa { get; set; }

    [Column("viDo")]
    public decimal? ViDo { get; set; }

    [Column("kinhDo")]
    public decimal? KinhDo { get; set; }

    [Column("ngayKhaiTruong")]
    [Display(Name = "Ngày khai trương")]
    public DateOnly? NgayKhaiTruong { get; set; }

    [Column("trangThai")]
    public byte TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TinhThanhId))]
    public TinhThanh? TinhThanh { get; set; }
    public ICollection<LopHoc> LopHocs { get; set; } = [];
    public ICollection<PhongHoc> PhongHocs { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: tinhthanh — Danh sách 63 tỉnh/thành phố Việt Nam
// ---------------------------------------------------------------------------
[Table("tinhthanh")]
public class TinhThanh
{
    [Key]
    [Column("tinhThanhId")]
    public int TinhThanhId { get; set; }

    // Mã API từ đơn vị hành chính Việt Nam
    [Column("maAPI")]
    public int? MaAPI { get; set; }

    [Required]
    [Column("tenTinhThanh")]
    [MaxLength(100)]
    [Display(Name = "Tỉnh/Thành phố")]
    public string TenTinhThanh { get; set; } = string.Empty;

    [Column("slug")]
    [MaxLength(100)]
    public string Slug { get; set; } = string.Empty;

    [Column("division_type")]
    [MaxLength(50)]
    public string? DivisionType { get; set; }

    [Column("codename")]
    [MaxLength(100)]
    public string? Codename { get; set; }

    public ICollection<CoSoDaoTao> CoSos { get; set; } = [];
}
