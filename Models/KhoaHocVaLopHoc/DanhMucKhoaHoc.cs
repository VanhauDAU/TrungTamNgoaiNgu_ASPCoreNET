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
[Table("danhmuckhoahoc")]
public class DanhMucKhoaHoc
{
    [Key]
    [Column("danhMucId")]
    public int DanhMucId { get; set; }

    /// <summary>Mã danh mục — VD: TA, TN. Tùy chọn để tra cứu nhanh.</summary>
    [Column("maDanhMuc")]
    [MaxLength(20)]
    [Display(Name = "Mã danh mục")]
    public string? MaDanhMuc { get; set; }

    [Required(ErrorMessage = "Tên danh mục không được để trống")]
    [Column("tenDanhMuc")]
    [MaxLength(100)]
    [Display(Name = "Tên danh mục")]
    public string TenDanhMuc { get; set; } = string.Empty;

    [Required]
    [Column("slug")]
    [MaxLength(100)]
    public string Slug { get; set; } = string.Empty;

    [Column("moTa")]
    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [Column("trangThai")]
    public byte TrangThai { get; set; } = 1;

    [Column("parent_id")]
    public int? ParentId { get; set; }

    /// <summary>Thứ tự hiển thị danh mục — số nhỏ hiển trước.</summary>
    [Column("sort_order")]
    public uint SortOrder { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Soft delete
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
    
    // Một danh mục có nhiều khóa học
    public ICollection<KhoaHoc> KhoaHocs { get; set; } = [];

    [ForeignKey(nameof(ParentId))]
    public DanhMucKhoaHoc? Parent { get; set; }

    public ICollection<DanhMucKhoaHoc> Children { get; set; } = [];

    [NotMapped]
    public bool IsDeleted => DeletedAt != null;
}

// ---------------------------------------------------------------------------
// BẢNG: khoahoc — Thông tin khóa học (IELTS, Tiếng Nhật sơ cấp...)
// ---------------------------------------------------------------------------
