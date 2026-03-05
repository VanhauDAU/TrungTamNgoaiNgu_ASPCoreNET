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

[Table("buoihoc")]
public class BuoiHoc
{
    [Key]
    [Column("buoiHocId")]
    public int BuoiHocId { get; set; }

    [Column("lopHocId")]
    public int? LopHocId { get; set; }

    [Column("tenBuoiHoc")]
    [MaxLength(255)]
    [Display(Name = "Tên buổi học")]
    public string? TenBuoiHoc { get; set; }

    [Column("ngayHoc")]
    [Display(Name = "Ngày học")]
    public DateOnly? NgayHoc { get; set; }

    [Column("caHocId")]
    public int? CaHocId { get; set; }

    [Column("phongHocId")]
    public int? PhongHocId { get; set; }

    // Giáo viên dạy buổi này
    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("ghiChu")]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    // Đã điểm danh chưa
    [Column("daDiemDanh")]
    public bool DaDiemDanh { get; set; }

    // Buổi học đã hoàn thành chưa
    [Column("daHoanThanh")]
    public bool DaHoanThanh { get; set; }

    [Column("trangThai")]
    public byte TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(LopHocId))]
    public LopHoc? LopHoc { get; set; }

    [ForeignKey(nameof(CaHocId))]
    public CaHoc? CaHoc { get; set; }

    [ForeignKey(nameof(PhongHocId))]
    public PhongHoc? PhongHoc { get; set; }

    public ICollection<DiemDanh> DiemDanhs { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: diemdanh — Điểm danh học viên theo từng buổi
// TrangThai: 0=Vắng | 1=Có mặt | 2=Đến trễ
// ---------------------------------------------------------------------------
