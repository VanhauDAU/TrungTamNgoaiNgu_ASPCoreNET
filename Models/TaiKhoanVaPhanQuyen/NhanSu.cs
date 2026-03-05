// =============================================================================
// NHÓM 1: TÀI KHOẢN & PHÂN QUYỀN
// Gồm: Tài khoản đăng nhập, Hồ sơ người dùng, Nhóm quyền, Phân quyền
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: taikhoan — Thông tin đăng nhập của tất cả người dùng
// Role: 0=Học viên | 1=Giáo viên | 2=Nhân viên | 3=Admin
// ---------------------------------------------------------------------------

[Table("nhansu")]
public class NhanSu
{
    [Key]
    [Column("taiKhoanId")]
    public int TaiKhoanId { get; set; }

    [Column("maNhanVien")]
    [MaxLength(20)]
    [Display(Name = "Mã nhân viên")]
    public string? MaNhanVien { get; set; }

    [Column("chucVu")]
    [MaxLength(50)]
    [Display(Name = "Chức vụ")]
    public string? ChucVu { get; set; }

    [Column("luongCoBan")]
    [Display(Name = "Lương cơ bản")]
    public decimal? LuongCoBan { get; set; }

    [Column("ngayVaoLam")]
    [Display(Name = "Ngày vào làm")]
    public DateOnly? NgayVaoLam { get; set; }

    [Column("chuyenMon")]
    [MaxLength(255)]
    [Display(Name = "Chuyên môn")]
    public string? ChuyenMon { get; set; }

    [Column("bangCap")]
    [MaxLength(255)]
    [Display(Name = "Bằng cấp")]
    public string? BangCap { get; set; }

    [Column("hocVi")]
    [MaxLength(100)]
    [Display(Name = "Học vị")]
    public string? HocVi { get; set; }

    [Column("coSoId")]
    [Display(Name = "Cơ sở")]
    public int? CoSoId { get; set; }

    [Column("loaiHopDong")]
    [MaxLength(255)]
    [Display(Name = "Loại hợp đồng")]
    public string? LoaiHopDong { get; set; }

    // 0: Đang làm | 1: Tạm nghỉ | 2: Đã nghỉ việc
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public byte? TrangThai { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [ForeignKey(nameof(CoSoId))]
    public CoSoDaoTao? CoSo { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: nhomquyen — Định nghĩa nhóm quyền (VD: Kế toán, Giáo viên...)
// ---------------------------------------------------------------------------
