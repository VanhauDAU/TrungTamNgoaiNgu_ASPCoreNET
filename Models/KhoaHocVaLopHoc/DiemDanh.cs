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

[Table("diemdanh")]
public class DiemDanh
{
    [Key]
    [Column("diemDanhId")]
    [MaxLength(50)]
    public string DiemDanhId { get; set; } = Guid.NewGuid().ToString();

    // Học viên nào
    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    // Buổi học nào
    [Column("buoiHocId")]
    public int? BuoiHocId { get; set; }

    // 0: Vắng | 1: Có mặt | 2: Đến trễ
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public byte? TrangThai { get; set; }

    [Column("ghiChu")]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [ForeignKey(nameof(BuoiHocId))]
    public BuoiHoc? BuoiHoc { get; set; }

    [NotMapped]
    public string TrangThaiText => TrangThai switch {
        0 => "Vắng mặt", 1 => "Có mặt", 2 => "Đến trễ", _ => "Chưa điểm danh"
    };
}
