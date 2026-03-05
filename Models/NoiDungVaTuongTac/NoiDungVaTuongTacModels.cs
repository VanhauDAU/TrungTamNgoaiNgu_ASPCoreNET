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
[Table("danhmucbaiviet")]
public class DanhMucBaiViet
{
    [Key]
    [Column("danhMucId")]
    public int DanhMucId { get; set; }

    [Required]
    [Column("tenDanhMuc")]
    [MaxLength(100)]
    [Display(Name = "Tên danh mục")]
    public string TenDanhMuc { get; set; } = string.Empty;

    [Required]
    [Column("slug")]
    [MaxLength(100)]
    public string Slug { get; set; } = string.Empty;

    [Column("moTa")]
    public string? MoTa { get; set; }

    [Column("trangThai")]
    public byte TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public ICollection<BaiViet> BaiViets { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: tags — Các nhãn bài viết (IELTS, TOEIC, Speaking...)
// ---------------------------------------------------------------------------
[Table("tags")]
public class Tag
{
    [Key]
    [Column("tagId")]
    public long TagId { get; set; }

    [Required]
    [Column("tenTag")]
    [MaxLength(50)]
    [Display(Name = "Tên tag")]
    public string TenTag { get; set; } = string.Empty;

    [Required]
    [Column("slug")]
    [MaxLength(50)]
    public string Slug { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public ICollection<BaiViet> BaiViets { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: baiviet — Bài viết / Blog của trung tâm
// TrangThai: 0=Ẩn | 1=Công khai
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
[Table("lienhe")]
public class LienHe
{
    [Key]
    [Column("lienHeId")]
    public int LienHeId { get; set; }

    [Column("hoTen")]
    [MaxLength(100)]
    [Display(Name = "Họ tên")]
    public string? HoTen { get; set; }

    [Column("email")]
    [MaxLength(100)]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Column("soDienThoai")]
    [MaxLength(15)]
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [Column("tieuDe")]
    [MaxLength(255)]
    [Display(Name = "Tiêu đề")]
    public string? TieuDe { get; set; }

    [Column("noiDung")]
    [Display(Name = "Nội dung")]
    public string? NoiDung { get; set; }

    // 0=Chưa xử lý | 1=Đã xử lý
    [Column("trangThai")]
    public byte? TrangThai { get; set; }

    [Column("loaiLienHe")]
    [MaxLength(20)]
    [Display(Name = "Loại liên hệ")]
    public string LoaiLienHe { get; set; } = "tu_van";

    [Column("ghiChuNoiBo")]
    [Display(Name = "Ghi chú nội bộ")]
    public string? GhiChuNoiBo { get; set; }

    [Column("nguoiPhuTrachId")]
    [Display(Name = "Người phụ trách")]
    public long? NguoiPhuTrachId { get; set; }

    [Column("thoiGianXuLy")]
    [Display(Name = "Thời gian xử lý")]
    public DateTime? ThoiGianXuLy { get; set; }

    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: lienhe_lichsu — Lịch sử xử lý liên hệ
// ---------------------------------------------------------------------------
[Table("lienhe_lichsu")]
public class LienHeLichSu
{
    [Key]
    [Column("lichSuId")]
    public long LichSuId { get; set; }

    [Column("lienHeId")]
    public long LienHeId { get; set; }

    [Required]
    [Column("hanhDong")]
    [MaxLength(100)]
    public string HanhDong { get; set; } = string.Empty;

    [Column("noiDung")]
    public string? NoiDung { get; set; }

    [Column("giaTriCu")]
    [MaxLength(200)]
    public string? GiaTriCu { get; set; }

    [Column("giaTriMoi")]
    [MaxLength(200)]
    public string? GiaTriMoi { get; set; }

    [Column("nguoiThucHienId")]
    public long? NguoiThucHienId { get; set; }

    [Column("tenNguoiThucHien")]
    [MaxLength(200)]
    public string? TenNguoiThucHien { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// ---------------------------------------------------------------------------
// BẢNG: lienhe_phanhoi — Nội dung phản hồi liên hệ
// ---------------------------------------------------------------------------
[Table("lienhe_phanhoi")]
public class LienHePhanHoi
{
    [Key]
    [Column("phanHoiId")]
    public long PhanHoiId { get; set; }

    [Column("lienHeId")]
    public long LienHeId { get; set; }

    [Required]
    [Column("noiDung")]
    public string NoiDung { get; set; } = string.Empty;

    [Required]
    [Column("loai")]
    [MaxLength(20)]
    public string Loai { get; set; } = "noi_bo";

    [Column("nguoiGuiId")]
    public long? NguoiGuiId { get; set; }

    [Column("tenNguoiGui")]
    [MaxLength(200)]
    public string? TenNguoiGui { get; set; }

    [Column("daGuiEmail")]
    public bool DaGuiEmail { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: thongbao — Thông báo gửi đến học viên / nhân viên
// LoaiGui: 0=Hệ thống | 1=Học tập | 2=Tài chính | 3=Sự kiện | 4=Khẩn cấp
// DoiTuongGui: 0=Tất cả | 1=Theo lớp | 2=Theo khóa học | 3=Cá nhân
// ---------------------------------------------------------------------------
[Table("thongbao")]
public class ThongBao
{
    [Key]
    [Column("thongBaoId")]
    public int ThongBaoId { get; set; }

    [Column("tieuDe")]
    [MaxLength(255)]
    [Display(Name = "Tiêu đề")]
    public string? TieuDe { get; set; }

    [Column("noiDung")]
    [Display(Name = "Nội dung")]
    public string? NoiDung { get; set; }

    // Người gửi thông báo
    [Column("nguoiGuiId")]
    public int? NguoiGuiId { get; set; }

    // 0=Tất cả | 1=Theo lớp | 2=Theo khóa | 3=Cá nhân
    [Column("doiTuongGui")]
    [Display(Name = "Đối tượng gửi")]
    public byte? DoiTuongGui { get; set; }

    [Column("doiTuongId")]
    public int? DoiTuongId { get; set; }

    // 0=Hệ thống | 1=Học tập | 2=Tài chính | 3=Sự kiện | 4=Khẩn cấp
    [Column("loaiGui")]
    [Display(Name = "Loại")]
    public byte LoaiGui { get; set; }

    // 0=Bình thường | 1=Quan trọng | 2=Khẩn cấp
    [Column("uuTien")]
    [Display(Name = "Ưu tiên")]
    public byte UuTien { get; set; }

    [Column("ghim")]
    [Display(Name = "Ghim")]
    public bool Ghim { get; set; }

    [Column("hinhAnh")]
    [MaxLength(255)]
    public string? HinhAnh { get; set; }

    [Column("ngayGui")]
    [Display(Name = "Ngày gửi")]
    public DateTime NgayGui { get; set; } = DateTime.Now;

    [Column("trangThai")]
    public byte? TrangThai { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(NguoiGuiId))]
    public TaiKhoan? NguoiGui { get; set; }
    public ICollection<ThongBaoNguoiDung> ThongBaoNguoiDungs { get; set; } = [];
    public ICollection<ThongBaoTepDinh> TepDinhs { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: thongbaonguoidung — Trạng thái đọc/chưa đọc của từng người
// ---------------------------------------------------------------------------
[Table("thongbaonguoidung")]
public class ThongBaoNguoiDung
{
    [Key]
    [Column("thongBaoNguoiDungId")]
    public int ThongBaoNguoiDungId { get; set; }

    [Column("thongBaoId")]
    public int? ThongBaoId { get; set; }

    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("daDoc")]
    [Display(Name = "Đã đọc")]
    public bool DaDoc { get; set; }

    [Column("ngayDoc")]
    public DateTime? NgayDoc { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(ThongBaoId))]
    public ThongBao? ThongBao { get; set; }

    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: thongbao_tepdinh — File đính kèm của thông báo
// ---------------------------------------------------------------------------
[Table("thongbao_tepdinh")]
public class ThongBaoTepDinh
{
    [Key]
    [Column("tepDinhId")]
    public long TepDinhId { get; set; }

    [Column("thongBaoId")]
    public int ThongBaoId { get; set; }

    [Required]
    [Column("tenFile")]
    [MaxLength(255)]
    public string TenFile { get; set; } = string.Empty;

    [Required]
    [Column("tenFileLuu")]
    [MaxLength(255)]
    public string TenFileLuu { get; set; } = string.Empty;

    [Required]
    [Column("duongDan")]
    [MaxLength(500)]
    public string DuongDan { get; set; } = string.Empty;

    [Column("loaiFile")]
    [MaxLength(100)]
    public string? LoaiFile { get; set; }

    [Column("kichThuoc")]
    public long KichThuoc { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(ThongBaoId))]
    public ThongBao? ThongBao { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: settings — Cài đặt hệ thống (tên trung tâm, logo, mạng xã hội...)
// ---------------------------------------------------------------------------
[Table("settings")]
public class Setting
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("key")]
    [MaxLength(191)]
    public string Key { get; set; } = string.Empty;

    [Column("value")]
    public string? Value { get; set; }

    [Required]
    [Column("display_name")]
    [MaxLength(191)]
    [Display(Name = "Tên hiển thị")]
    public string DisplayName { get; set; } = string.Empty;

    // text | textarea | image | color
    [Column("type")]
    [MaxLength(50)]
    [Display(Name = "Kiểu dữ liệu")]
    public string Type { get; set; } = "text";

    // general | social | seo | theme
    [Column("group")]
    [MaxLength(50)]
    [Display(Name = "Nhóm cài đặt")]
    public string Group { get; set; } = "general";

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}
