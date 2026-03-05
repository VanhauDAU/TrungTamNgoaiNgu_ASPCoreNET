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

[Table("hocphi")]
public class HocPhi
{
    [Key]
    [Column("hocPhiId")]
    public long HocPhiId { get; set; }

    [Column("khoaHocId")]
    public int? KhoaHocId { get; set; }

    [Column("soBuoi")]
    [Display(Name = "Số buổi")]
    public int? SoBuoi { get; set; }

    [Column("donGia")]
    [Display(Name = "Đơn giá / buổi")]
    public decimal? DonGia { get; set; }

    [Column("trangThai")]
    public byte TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(KhoaHocId))]
    public KhoaHoc? KhoaHoc { get; set; }

    // Tổng học phí = số buổi x đơn giá
    [NotMapped]
    public decimal TongHocPhi => (SoBuoi ?? 0) * (DonGia ?? 0);
}

// ---------------------------------------------------------------------------
// BẢNG: cahoc — Ca học (Ca sáng 8h-10h, Ca tối 18h-20h...)
// ---------------------------------------------------------------------------
