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

[Table("lophoc")]
public class LopHoc
{
    [Key]
    [Column("lopHocId")]
    public int LopHocId { get; set; }

    [Column("slug")]
    [MaxLength(255)]
    public string? Slug { get; set; }

    [Column("tenLopHoc")]
    [MaxLength(255)]
    [Display(Name = "Tên lớp")]
    public string? TenLopHoc { get; set; }

    [Column("khoaHocId")]
    public int? KhoaHocId { get; set; }

    [Column("phongHocId")]
    public int? PhongHocId { get; set; }

    // Giáo viên phụ trách lớp
    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("hocPhiId")]
    public long? HocPhiId { get; set; }

    [Column("ngayBatDau")]
    [Display(Name = "Ngày bắt đầu")]
    public DateOnly? NgayBatDau { get; set; }

    [Column("ngayKetThuc")]
    [Display(Name = "Ngày kết thúc")]
    public DateOnly? NgayKetThuc { get; set; }

    [Column("soBuoiDuKien")]
    [Display(Name = "Số buổi dự kiến")]
    public int? SoBuoiDuKien { get; set; }

    [Column("soHocVienToiDa")]
    [Display(Name = "Sĩ số tối đa")]
    public int? SoHocVienToiDa { get; set; }

    [Column("donGiaDay")]
    [Display(Name = "Đơn giá dạy / buổi")]
    public decimal? DonGiaDay { get; set; }

    // Lịch học dạng JSON, VD: "2,5" = Thứ 2 và Thứ 5
    [Column("lichHoc")]
    [Display(Name = "Lịch học")]
    public string? LichHoc { get; set; }

    [Column("coSoId")]
    public int? CoSoId { get; set; }

    [Column("caHocId")]
    public int CaHocId { get; set; }

    // 0=Sắp mở | 1=Đang mở | 2=Đã đóng | 3=Đã hủy | 4=Đang học
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public byte? TrangThai { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(KhoaHocId))]
    public KhoaHoc? KhoaHoc { get; set; }

    [ForeignKey(nameof(PhongHocId))]
    public PhongHoc? PhongHoc { get; set; }

    [ForeignKey(nameof(CoSoId))]
    public CoSoDaoTao? CoSo { get; set; }

    [ForeignKey(nameof(CaHocId))]
    public CaHoc? CaHoc { get; set; }

    public ICollection<DangKyLopHoc> DangKys { get; set; } = [];
    public ICollection<BuoiHoc> BuoiHocs { get; set; } = [];

    [NotMapped]
    public string TrangThaiText => TrangThai switch {
        0 => "Sắp mở", 1 => "Đang mở", 2 => "Đã đóng",
        3 => "Đã hủy", 4 => "Đang học", _ => "Không xác định"
    };
}

// ---------------------------------------------------------------------------
// BẢNG: dangkylophoc — Học viên đăng ký vào lớp
// TrangThai: 1=Đang học | 2=Đã hoàn thành | 3=Đã hủy
// ---------------------------------------------------------------------------
