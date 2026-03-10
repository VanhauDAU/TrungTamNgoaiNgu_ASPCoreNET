// =============================================================================
// NHÓM 2: KHÓA HỌC & LỚP HỌC
// Gồm: Khóa học, Danh mục, Học phí, Lớp học, Ca học, Phòng học, Buổi học
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrungTamNgoaiNgu.Enums;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: buoihoc — Từng buổi học của lớp
//
// TRẠNG THÁI (TrangThai): xem enum BuoiHocTrangThai
//   0 SAP_DIEN_RA   – Chưa đến giờ, chưa diễn ra
//   1 DANG_DIEN_RA  – Đang trong giờ học
//   2 DA_HOAN_THANH – Kết thúc, đã điểm danh xong
//   3 DA_HUY        – Buổi học bị hủy
//   4 DOI_LICH      – Dời sang ngày khác
//
// LưU Ý: daDiemDanh và daHoanThanh là cờ riêng, KHÔNG phải TrangThai:
//   daDiemDanh  = true khi giáo viên đã bấm "Hoàn tất điểm danh"
//   daHoanThanh = true khi buổi chuyển sang DA_HOAN_THANH
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

    /// <summary>
    /// Trạng thái buổi học — xem <see cref="BuoiHocTrangThai"/>.
    /// </summary>
    [Column("trangThai")]
    public BuoiHocTrangThai TrangThai { get; set; } = BuoiHocTrangThai.SapDienRa;

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

    // ---------------------------------------------------------------------------
    // Computed properties (NotMapped)
    // ---------------------------------------------------------------------------

    [NotMapped]
    public string TrangThaiText => TrangThai.GetLabel();

    [NotMapped]
    public string TrangThaiBadgeClass => TrangThai.GetBadgeClass();

    [NotMapped]
    public string TrangThaiIcon => TrangThai.GetIcon();

    /// <summary>Buổi học có thể điểm danh được không?</summary>
    [NotMapped]
    public bool CoTheHoc => TrangThai.CoTheHoc();
}

