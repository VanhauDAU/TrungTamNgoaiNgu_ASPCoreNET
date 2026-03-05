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

[Table("phonghoc")]
public class PhongHoc
{
    [Key]
    [Column("phongHocId")]
    public int PhongHocId { get; set; }

    [Column("tenPhong")]
    [MaxLength(100)]
    [Display(Name = "Tên phòng")]
    public string? TenPhong { get; set; }

    [Column("sucChua")]
    [Display(Name = "Sức chứa (người)")]
    public int? SucChua { get; set; }

    [Column("trangThietBi")]
    [Display(Name = "Trang thiết bị")]
    public string? TrangThietBi { get; set; }

    [Column("coSoId")]
    public int? CoSoId { get; set; }

    [Column("trangThai")]
    public int TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(CoSoId))]
    public CoSoDaoTao? CoSo { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: lophoc — Lớp học cụ thể (một khóa học có thể mở nhiều lớp)
// TrangThai: 0=Sắp mở | 1=Đang mở | 2=Đã đóng | 3=Đã hủy | 4=Đang học
// ---------------------------------------------------------------------------
