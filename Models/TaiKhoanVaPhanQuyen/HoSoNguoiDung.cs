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

[Table("hosonguoidung")]
public class HoSoNguoiDung
{
    // Khóa chính đồng thời là khóa ngoại trỏ đến TaiKhoan
    [Key]
    [Column("taiKhoanId")]
    public int TaiKhoanId { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống")]
    [Column("hoTen")]
    [MaxLength(100)]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = string.Empty;

    [Column("soDienThoai")]
    [MaxLength(15)]
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [Column("zalo")]
    [MaxLength(20)]
    public string? Zalo { get; set; }

    [Column("ngaySinh")]
    [Display(Name = "Ngày sinh")]
    public DateOnly? NgaySinh { get; set; }

    // 0: Nữ | 1: Nam | 2: Khác
    [Column("gioiTinh")]
    [Display(Name = "Giới tính")]
    public byte? GioiTinh { get; set; }

    [Column("diaChi")]
    [MaxLength(255)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    // Thông tin người giám hộ (dành cho học viên nhỏ tuổi)
    [Column("nguoiGiamHo")]
    [MaxLength(100)]
    [Display(Name = "Người giám hộ")]
    public string? NguoiGiamHo { get; set; }

    [Column("sdtGuardian")]
    [MaxLength(20)]
    [Display(Name = "SĐT người giám hộ")]
    public string? SdtGuardian { get; set; }

    [Column("moiQuanHe")]
    [MaxLength(50)]
    [Display(Name = "Mối quan hệ")]
    public string? MoiQuanHe { get; set; }

    [Column("trinhDoHienTai")]
    [MaxLength(30)]
    [Display(Name = "Trình độ hiện tại")]
    public string? TrinhDoHienTai { get; set; }

    [Column("ngonNguMucTieu")]
    [MaxLength(50)]
    [Display(Name = "Ngôn ngữ mục tiêu")]
    public string? NgonNguMucTieu { get; set; }

    [Column("nguonBietDen")]
    [MaxLength(50)]
    [Display(Name = "Nguồn biết đến")]
    public string? NguonBietDen { get; set; }

    [Column("ghiChu")]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    [Column("cccd")]
    [MaxLength(20)]
    [Display(Name = "CCCD/CMND")]
    public string? Cccd { get; set; }

    [Column("anhDaiDien")]
    [MaxLength(255)]
    public string? AnhDaiDien { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [NotMapped]
    public string GioiTinhText => GioiTinh switch {
        0 => "Nữ", 1 => "Nam", 2 => "Khác", _ => "Chưa cập nhật"
    };
}

// ---------------------------------------------------------------------------
// BẢNG: nhansu — Hồ sơ nhân sự (giáo viên, nhân viên)
// ---------------------------------------------------------------------------
