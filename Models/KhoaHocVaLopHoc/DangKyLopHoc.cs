// =============================================================================
// NHÓM 2: KHÓA HỌC & LỚP HỌC
// Gồm: Khóa học, Danh mục, Học phí, Lớp học, Ca học, Phòng học, Buổi học
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrungTamNgoaiNgu.Enums;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: dangkylophoc — Học viên đăng ký vào lớp
//
// TRẠNG THÁI (TrangThai): xem enum DangKyTrangThai
//   0 CHO_THANH_TOAN         – Chưa thanh toán học phí
//   1 DA_XAC_NHAN            – Đã thanh toán, chưa khai giảng
//   2 DANG_HOC               – Đang tham gia học
//   3 TAM_DUNG_NO_HOC_PHI    – Tạm dừng do nợ học phí
//   4 BAO_LUU                – Bảo lưu (giữ quyền học sau)
//   5 HOAN_THANH             – Hoàn thành khóa học
//   6 HUY                   – Đã hủy đăng ký
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

    /// <summary>
    /// Trạng thái đăng ký lớp học — xem <see cref="DangKyTrangThai"/>.
    /// DB lưu dạng byte (0-6).
    /// </summary>
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public DangKyTrangThai TrangThai { get; set; } = DangKyTrangThai.ChoThanhToan;

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

    // ---------------------------------------------------------------------------
    // Computed properties (NotMapped)
    // ---------------------------------------------------------------------------

    [NotMapped]
    public string TrangThaiText => TrangThai.GetLabel();

    [NotMapped]
    public string TrangThaiBadgeClass => TrangThai.GetBadgeClass();

    [NotMapped]
    public string TrangThaiIcon => TrangThai.GetIcon();

    /// <summary>Học viên có được phép điểm danh không (không bị khóa)?</summary>
    [NotMapped]
    public bool CoTheHoc => TrangThai.CoTheHoc();

    /// <summary>Bị khóa do nợ học phí?</summary>
    [NotMapped]
    public bool BiKhoaDiemDanh => TrangThai.BiKhoaDiemDanh();
}

// ---------------------------------------------------------------------------
// BẢNG: buoihoc — Từng buổi học cụ thể trong lớp
// ---------------------------------------------------------------------------
