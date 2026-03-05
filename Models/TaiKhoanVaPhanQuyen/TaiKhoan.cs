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

[Table("taikhoan")]
public class TaiKhoan
{
    [Key]
    [Column("taiKhoanId")]
    public int TaiKhoanId { get; set; }

    [Required(ErrorMessage = "Tên tài khoản không được để trống")]
    [Column("taiKhoan")]
    [MaxLength(50)]
    [Display(Name = "Tài khoản")]
    public string TenTaiKhoan { get; set; } = string.Empty;

    [Required]
    [Column("matKhau")]
    [MaxLength(255)]
    public string MatKhau { get; set; } = string.Empty;

    [Column("email")]
    [MaxLength(50)]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    // 0: Học viên | 1: Giáo viên | 2: Nhân viên | 3: Admin
    [Column("role")]
    [Display(Name = "Vai trò")]
    public byte Role { get; set; }

    // Chỉ dùng cho Role 1 (GV) và 2 (NV) — thuộc nhóm quyền nào
    [Column("nhomQuyenId")]
    public int? NhomQuyenId { get; set; }

    // 0: Bị khóa | 1: Hoạt động | 2: Chờ kích hoạt
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public byte TrangThai { get; set; } = 1;

    [Column("lastLogin")]
    [Display(Name = "Đăng nhập lần cuối")]
    public DateTime? LastLogin { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    // --- LIÊN KẾT (Navigation Properties) ---
    // Mỗi tài khoản có 1 hồ sơ cá nhân
    public HoSoNguoiDung? HoSo { get; set; }
    // Nếu là giáo viên/nhân viên, có thêm hồ sơ nhân sự
    public NhanSu? NhanSu { get; set; }
    // Thuộc nhóm quyền nào (nếu là GV/NV)
    [ForeignKey(nameof(NhomQuyenId))]
    public NhomQuyen? NhomQuyen { get; set; }

    // --- THUỘC TÍNH TÍNH TOÁN (không lưu vào DB) ---
    [NotMapped]
    public string RoleName => Role switch {
        0 => "Học viên", 1 => "Giáo viên", 2 => "Nhân viên", 3 => "Admin",
        _ => "Không xác định"
    };

    [NotMapped]
    public string TrangThaiText => TrangThai switch {
        0 => "Bị khóa", 1 => "Hoạt động", 2 => "Chờ kích hoạt",
        _ => "Không xác định"
    };

    [NotMapped]
    public bool IsActive => TrangThai == 1 && DeletedAt == null;
}

// ---------------------------------------------------------------------------
// BẢNG: hosonguoidung — Thông tin cá nhân chi tiết
// ---------------------------------------------------------------------------
