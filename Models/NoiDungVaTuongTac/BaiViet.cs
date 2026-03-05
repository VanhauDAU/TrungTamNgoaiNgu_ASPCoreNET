// =============================================================================
// NHÓM 5: NỘI DUNG & TƯƠNG TÁC
// Gồm: Bài viết, Danh mục bài viết, Tags, Liên hệ, Thông báo
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: danhmucbaiviet — Danh mục phân loại bài viết
// ---------------------------------------------------------------------------

[Table("baiviet")]
public class BaiViet
{
    [Key]
    [Column("baiVietId")]
    public int BaiVietId { get; set; }

    [Required(ErrorMessage = "Tiêu đề không được để trống")]
    [Column("tieuDe")]
    [MaxLength(255)]
    [Display(Name = "Tiêu đề")]
    public string TieuDe { get; set; } = string.Empty;

    [Required]
    [Column("slug")]
    [MaxLength(255)]
    public string Slug { get; set; } = string.Empty;

    [Column("tomTat")]
    [Display(Name = "Tóm tắt")]
    public string? TomTat { get; set; }

    [Required]
    [Column("noiDung")]
    [Display(Name = "Nội dung")]
    public string NoiDung { get; set; } = string.Empty;

    [Column("anhDaiDien")]
    [MaxLength(255)]
    [Display(Name = "Ảnh đại diện")]
    public string? AnhDaiDien { get; set; }

    // Tác giả bài viết
    [Column("taiKhoanId")]
    public int TaiKhoanId { get; set; }

    [Column("luotXem")]
    [Display(Name = "Lượt xem")]
    public int LuotXem { get; set; }

    // 0: Ẩn | 1: Công khai
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public byte TrangThai { get; set; }

    [Column("published_at")]
    [Display(Name = "Ngày đăng")]
    public DateTime? PublishedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TacGia { get; set; }

    // Many-to-many với DanhMucBaiViet và Tag thông qua bảng trung gian
    public ICollection<DanhMucBaiViet> DanhMucs { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: lienhe — Liên hệ / đăng ký tư vấn từ khách hàng
// TrangThai: 0=Chưa xử lý | 1=Đã xử lý
// ---------------------------------------------------------------------------
