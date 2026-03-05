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

[Table("nhomquyen")]
public class NhomQuyen
{
    [Key]
    [Column("nhomQuyenId")]
    public int NhomQuyenId { get; set; }

    [Required]
    [Column("tenNhom")]
    [MaxLength(100)]
    [Display(Name = "Tên nhóm")]
    public string TenNhom { get; set; } = string.Empty;

    [Column("moTa")]
    [MaxLength(255)]
    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    public ICollection<PhanQuyen> PhanQuyens { get; set; } = [];
    public ICollection<TaiKhoan> TaiKhoans { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: phanquyen — Chi tiết quyền của từng nhóm trên từng tính năng
// Tính năng: khoa_hoc | lop_hoc | hoc_vien | giao_vien | nhan_vien | tai_chinh...
// ---------------------------------------------------------------------------
