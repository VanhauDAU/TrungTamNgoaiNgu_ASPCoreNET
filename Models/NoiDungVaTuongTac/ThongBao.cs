// =============================================================================
// NHÓM 5: NỘI DUNG & TƯƠNG TÁC
// Gồm: Bài viết, Danh mục bài viết, Tags, Liên hệ, Thông báo
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: danhmucbaiviet — Danh mục phân loại bài viết
// ---------------------------------------------------------------------------

[Table("thongbao")]
public class ThongBao
{
    [Key]
    [Column("thongBaoId")]
    public long ThongBaoId { get; set; }

    [Column("tieuDe")]
    [MaxLength(255)]
    [Display(Name = "Tiêu đề")]
    public string? TieuDe { get; set; }

    [Column("noiDung")]
    [Display(Name = "Nội dung")]
    public string? NoiDung { get; set; }

    // Người gửi thông báo
    [Column("nguoiGuiId")]
    public int? NguoiGuiId { get; set; }

    [Column("loaiThongBao")]
    public byte? LoaiThongBao { get; set; }

    // 0=Tất cả | 1=Theo lớp | 2=Theo khóa | 3=Cá nhân
    [Column("doiTuongGui")]
    [Display(Name = "Đối tượng gửi")]
    public byte? DoiTuongGui { get; set; }

    [Column("doiTuongId")]
    public long? DoiTuongId { get; set; }

    // 0=Hệ thống | 1=Học tập | 2=Tài chính | 3=Sự kiện | 4=Khẩn cấp
    [Column("loaiGui")]
    [Display(Name = "Loại")]
    public byte LoaiGui { get; set; }

    // 0=Bình thường | 1=Quan trọng | 2=Khẩn cấp
    [Column("uuTien")]
    [Display(Name = "Ưu tiên")]
    public byte UuTien { get; set; }

    [Column("ghim")]
    [Display(Name = "Ghim")]
    public bool Ghim { get; set; }

    [Column("hinhAnh")]
    [MaxLength(255)]
    public string? HinhAnh { get; set; }

    [Column("ngayGui")]
    [Display(Name = "Ngày gửi")]
    public DateTime NgayGui { get; set; } = DateTime.Now;

    [Column("trangThai")]
    public byte? TrangThai { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(NguoiGuiId))]
    public TaiKhoan? NguoiGui { get; set; }
    public ICollection<ThongBaoNguoiDung> ThongBaoNguoiDungs { get; set; } = [];
    public ICollection<ThongBaoTepDinh> TepDinhs { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: thongbaonguoidung — Trạng thái đọc/chưa đọc của từng người
// ---------------------------------------------------------------------------
