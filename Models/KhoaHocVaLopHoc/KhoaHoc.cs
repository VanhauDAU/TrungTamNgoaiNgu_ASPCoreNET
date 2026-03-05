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

[Table("khoahoc")]
public class KhoaHoc
{
    [Key]
    [Column("khoaHocId")]
    public int KhoaHocId { get; set; }

    [Column("danhMucId")]
    public int? DanhMucId { get; set; }

    [Required(ErrorMessage = "Tên khóa học không được để trống")]
    [Column("tenKhoaHoc")]
    [MaxLength(255)]
    [Display(Name = "Tên khóa học")]
    public string TenKhoaHoc { get; set; } = string.Empty;

    [Required]
    [Column("slug")]
    [MaxLength(255)]
    public string Slug { get; set; } = string.Empty;

    [Column("anhKhoaHoc")]
    [MaxLength(255)]
    [Display(Name = "Ảnh đại diện")]
    public string? AnhKhoaHoc { get; set; }

    [Column("moTa")]
    [Display(Name = "Mô tả ngắn")]
    public string? MoTa { get; set; }

    [Column("doiTuong")]
    [Display(Name = "Đối tượng học viên")]
    public string? DoiTuong { get; set; }

    [Column("ketQuaDatDuoc")]
    [Display(Name = "Kết quả sau khóa học")]
    public string? KetQuaDatDuoc { get; set; }

    [Column("yeuCauDauVao")]
    [Display(Name = "Yêu cầu đầu vào")]
    public string? YeuCauDauVao { get; set; }

    // 0: Ẩn | 1: Công khai
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public byte TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Soft delete — không xóa thật khỏi DB, chỉ đánh dấu ngày xóa
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(DanhMucId))]
    public DanhMucKhoaHoc? DanhMuc { get; set; }
    public ICollection<LopHoc> LopHocs { get; set; } = [];
    public ICollection<HocPhi> HocPhis { get; set; } = [];

    [NotMapped]
    public bool IsDeleted => DeletedAt != null;
}

// ---------------------------------------------------------------------------
// BẢNG: hocphi — Bảng giá học phí theo số buổi
// Ví dụ: 35 buổi x 50.000đ/buổi
// ---------------------------------------------------------------------------
