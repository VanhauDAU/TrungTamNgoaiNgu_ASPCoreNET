// =============================================================================
// NHÓM 2: KHÓA HỌC & LỚP HỌC
// Gồm: Khóa học, Danh mục, Học phí, Lớp học, Ca học, Phòng học, Buổi học
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: danhmuckhoahoc — Danh mục phân loại khóa học (VD: Tiếng Anh, Tiếng Nhật)
// ---------------------------------------------------------------------------

[Table("dangkylophoc")]
public class DangKyLopHoc
{
    [Key]
    [Column("dangKyLopHocId")]
    public int DangKyLopHocId { get; set; }

    // Học viên nào đăng ký
    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    // Đăng ký lớp nào
    [Column("lopHocId")]
    public int? LopHocId { get; set; }

    [Column("ngayDangKy")]
    [Display(Name = "Ngày đăng ký")]
    public DateOnly? NgayDangKy { get; set; }

    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public int? TrangThai { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [ForeignKey(nameof(LopHocId))]
    public LopHoc? LopHoc { get; set; }

    public HoaDon? HoaDon { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: buoihoc — Từng buổi học cụ thể trong lớp
// ---------------------------------------------------------------------------
