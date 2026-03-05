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

[Table("phanquyen")]
public class PhanQuyen
{
    [Key]
    [Column("phanQuyenId")]
    public int PhanQuyenId { get; set; }

    [Column("nhomQuyenId")]
    public int NhomQuyenId { get; set; }

    [Column("tinhNang")]
    [MaxLength(50)]
    [Display(Name = "Tính năng")]
    public string TinhNang { get; set; } = string.Empty;

    [Column("coXem")]
    [Display(Name = "Xem")]
    public bool CoXem { get; set; }

    [Column("coThem")]
    [Display(Name = "Thêm")]
    public bool CoThem { get; set; }

    [Column("coSua")]
    [Display(Name = "Sửa")]
    public bool CoSua { get; set; }

    [Column("coXoa")]
    [Display(Name = "Xóa")]
    public bool CoXoa { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(NhomQuyenId))]
    public NhomQuyen? NhomQuyen { get; set; }
}
