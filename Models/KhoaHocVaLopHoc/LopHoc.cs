// =============================================================================
// NHÓM 2: KHÓA HỌC & LỚP HỌC
// Gồm: Khóa học, Danh mục, Học phí, Lớp học, Ca học, Phòng học, Buổi học
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrungTamNgoaiNgu.Enums;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: lophoc — Lớp học cụ thể (gắn với khóa học, ca học, cơ sở, giáo viên)
//
// TRẠNG THÁI (TrangThai): xem enum LopHocTrangThai
//   0 SAP_MO          – Tạo xong, chưa mở đăng ký
//   1 DANG_TUYEN_SINH – Đang nhận đăng ký học viên
//   2 CHOT_DANH_SACH  – Đã chốt, không nhận thêm
//   3 DA_HUY          – Lớp bị hủy
//   4 DANG_HOC        – Đã khai giảng, đang học
//   5 DA_KET_THUC     – Kết thúc toàn bộ buổi học
// ---------------------------------------------------------------------------

[Table("lophoc")]
public class LopHoc
{
    [Key]
    [Column("lopHocId")]
    public int LopHocId { get; set; }

    [Column("maLopHoc")]
    [MaxLength(20)]
    [Display(Name = "Mã lớp")]
    public string? MaLopHoc { get; set; }

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

    /// <summary>
    /// Trạng thái lớp học — xem <see cref="LopHocTrangThai"/> để biết các giá trị.
    /// DB lưu dạng byte (0-5).
    /// </summary>
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public LopHocTrangThai TrangThai { get; set; } = LopHocTrangThai.SapMo;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

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

    // ---------------------------------------------------------------------------
    // Computed properties (NotMapped — không lưu DB)
    // ---------------------------------------------------------------------------

    /// <summary>Nhãn hiển thị của trạng thái (lấy từ [Display(Name)]).</summary>
    [NotMapped]
    public string TrangThaiText => TrangThai.GetLabel();

    /// <summary>CSS class Bootstrap badge theo trạng thái.</summary>
    [NotMapped]
    public string TrangThaiBadgeClass => TrangThai.GetBadgeClass();

    /// <summary>Bootstrap Icon class theo trạng thái.</summary>
    [NotMapped]
    public string TrangThaiIcon => TrangThai.GetIcon();

    /// <summary>Lớp đang nhận đăng ký mới không?</summary>
    [NotMapped]
    public bool DangNhanDangKy => TrangThai.DangNhanDangKy();
}

// ---------------------------------------------------------------------------
// BẢNG: dangkylophoc — Học viên đăng ký vào lớp
// TrangThai: 1=Đang học | 2=Đã hoàn thành | 3=Đã hủy
// ---------------------------------------------------------------------------
