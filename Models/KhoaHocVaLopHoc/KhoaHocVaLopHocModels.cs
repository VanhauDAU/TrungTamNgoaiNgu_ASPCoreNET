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
[Table("cahoc")]
public class CaHoc
{
    [Key]
    [Column("caHocId")]
    public int CaHocId { get; set; }

    [Column("tenCa")]
    [MaxLength(100)]
    [Display(Name = "Tên ca")]
    public string? TenCa { get; set; }

    [Column("gioBatDau")]
    [Display(Name = "Giờ bắt đầu")]
    public TimeOnly? GioBatDau { get; set; }

    [Column("gioKetThuc")]
    [Display(Name = "Giờ kết thúc")]
    public TimeOnly? GioKetThuc { get; set; }

    [Column("moTa")]
    [MaxLength(255)]
    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [Column("trangThai")]
    public byte TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

// ---------------------------------------------------------------------------
// BẢNG: phonghoc — Phòng học tại từng cơ sở
// ---------------------------------------------------------------------------
[Table("phonghoc")]
public class PhongHoc
{
    [Key]
    [Column("phongHocId")]
    public int PhongHocId { get; set; }

    [Column("tenPhong")]
    [MaxLength(100)]
    [Display(Name = "Tên phòng")]
    public string? TenPhong { get; set; }

    [Column("sucChua")]
    [Display(Name = "Sức chứa (người)")]
    public int? SucChua { get; set; }

    [Column("trangThietBi")]
    [Display(Name = "Trang thiết bị")]
    public string? TrangThietBi { get; set; }

    [Column("coSoId")]
    public int? CoSoId { get; set; }

    [Column("trangThai")]
    public int TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(CoSoId))]
    public CoSoDaoTao? CoSo { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: lophoc — Lớp học cụ thể (một khóa học có thể mở nhiều lớp)
// TrangThai: 0=Sắp mở | 1=Đang mở | 2=Đã đóng | 3=Đã hủy | 4=Đang học
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

    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public int? TrangThai { get; set; }

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
}

// ---------------------------------------------------------------------------
// BẢNG: buoihoc — Từng buổi học cụ thể trong lớp
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
[Table("diemdanh")]
public class DiemDanh
{
    [Key]
    [Column("diemDanhId")]
    [MaxLength(50)]
    public string DiemDanhId { get; set; } = Guid.NewGuid().ToString();

    // Học viên nào
    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    // Buổi học nào
    [Column("buoiHocId")]
    public int? BuoiHocId { get; set; }

    // 0: Vắng | 1: Có mặt | 2: Đến trễ
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public byte? TrangThai { get; set; }

    [Column("ghiChu")]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [ForeignKey(nameof(BuoiHocId))]
    public BuoiHoc? BuoiHoc { get; set; }

    [NotMapped]
    public string TrangThaiText => TrangThai switch {
        0 => "Vắng mặt", 1 => "Có mặt", 2 => "Đến trễ", _ => "Chưa điểm danh"
    };
}
