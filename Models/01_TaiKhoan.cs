// =============================================================================
// NHÓM 1: TÀI KHOẢN & PHÂN QUYỀN
// Gồm: Tài khoản đăng nhập, Hồ sơ người dùng, Nhóm quyền, Phân quyền
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: taikhoan — Thông tin đăng nhập của tất cả người dùng
// Role: 0=Học viên | 1=Giáo viên | 2=Nhân viên | 3=Admin
// ---------------------------------------------------------------------------
[Table("taikhoan")]
public class TaiKhoan
{
    [Key]
    [Column("taiKhoanId")]
    public int TaiKhoanId { get; set; }

    [Required(ErrorMessage = "Tên tài khoản không được để trống")]
    [Column("taiKhoan")]
    [MaxLength(50)]
    [Display(Name = "Tài khoản")]
    public string TenTaiKhoan { get; set; } = string.Empty;

    [Required]
    [Column("matKhau")]
    [MaxLength(255)]
    public string MatKhau { get; set; } = string.Empty;

    [Column("email")]
    [MaxLength(50)]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    // 0: Học viên | 1: Giáo viên | 2: Nhân viên | 3: Admin
    [Column("role")]
    [Display(Name = "Vai trò")]
    public byte Role { get; set; }

    // Chỉ dùng cho Role 1 (GV) và 2 (NV) — thuộc nhóm quyền nào
    [Column("nhomQuyenId")]
    public int? NhomQuyenId { get; set; }

    // 0: Bị khóa | 1: Hoạt động | 2: Chờ kích hoạt
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public byte TrangThai { get; set; } = 1;

    [Column("lastLogin")]
    [Display(Name = "Đăng nhập lần cuối")]
    public DateTime? LastLogin { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    // --- LIÊN KẾT (Navigation Properties) ---
    // Mỗi tài khoản có 1 hồ sơ cá nhân
    public HoSoNguoiDung? HoSo { get; set; }
    // Nếu là giáo viên/nhân viên, có thêm hồ sơ nhân sự
    public NhanSu? NhanSu { get; set; }
    // Thuộc nhóm quyền nào (nếu là GV/NV)
    [ForeignKey(nameof(NhomQuyenId))]
    public NhomQuyen? NhomQuyen { get; set; }

    // --- THUỘC TÍNH TÍNH TOÁN (không lưu vào DB) ---
    [NotMapped]
    public string RoleName => Role switch {
        0 => "Học viên", 1 => "Giáo viên", 2 => "Nhân viên", 3 => "Admin",
        _ => "Không xác định"
    };

    [NotMapped]
    public string TrangThaiText => TrangThai switch {
        0 => "Bị khóa", 1 => "Hoạt động", 2 => "Chờ kích hoạt",
        _ => "Không xác định"
    };

    [NotMapped]
    public bool IsActive => TrangThai == 1 && DeletedAt == null;
}

// ---------------------------------------------------------------------------
// BẢNG: hosonguoidung — Thông tin cá nhân chi tiết
// ---------------------------------------------------------------------------
[Table("hosonguoidung")]
public class HoSoNguoiDung
{
    // Khóa chính đồng thời là khóa ngoại trỏ đến TaiKhoan
    [Key]
    [Column("taiKhoanId")]
    public int TaiKhoanId { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống")]
    [Column("hoTen")]
    [MaxLength(100)]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = string.Empty;

    [Column("soDienThoai")]
    [MaxLength(15)]
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [Column("zalo")]
    [MaxLength(20)]
    public string? Zalo { get; set; }

    [Column("ngaySinh")]
    [Display(Name = "Ngày sinh")]
    public DateOnly? NgaySinh { get; set; }

    // 0: Nữ | 1: Nam | 2: Khác
    [Column("gioiTinh")]
    [Display(Name = "Giới tính")]
    public byte? GioiTinh { get; set; }

    [Column("diaChi")]
    [MaxLength(255)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    // Thông tin người giám hộ (dành cho học viên nhỏ tuổi)
    [Column("nguoiGiamHo")]
    [MaxLength(100)]
    [Display(Name = "Người giám hộ")]
    public string? NguoiGiamHo { get; set; }

    [Column("sdtGuardian")]
    [MaxLength(20)]
    [Display(Name = "SĐT người giám hộ")]
    public string? SdtGuardian { get; set; }

    [Column("moiQuanHe")]
    [MaxLength(50)]
    [Display(Name = "Mối quan hệ")]
    public string? MoiQuanHe { get; set; }

    [Column("trinhDoHienTai")]
    [MaxLength(30)]
    [Display(Name = "Trình độ hiện tại")]
    public string? TrinhDoHienTai { get; set; }

    [Column("ngonNguMucTieu")]
    [MaxLength(50)]
    [Display(Name = "Ngôn ngữ mục tiêu")]
    public string? NgonNguMucTieu { get; set; }

    [Column("nguonBietDen")]
    [MaxLength(50)]
    [Display(Name = "Nguồn biết đến")]
    public string? NguonBietDen { get; set; }

    [Column("ghiChu")]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    [Column("cccd")]
    [MaxLength(20)]
    [Display(Name = "CCCD/CMND")]
    public string? Cccd { get; set; }

    [Column("anhDaiDien")]
    [MaxLength(255)]
    public string? AnhDaiDien { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [NotMapped]
    public string GioiTinhText => GioiTinh switch {
        0 => "Nữ", 1 => "Nam", 2 => "Khác", _ => "Chưa cập nhật"
    };
}

// ---------------------------------------------------------------------------
// BẢNG: nhansu — Hồ sơ nhân sự (giáo viên, nhân viên)
// ---------------------------------------------------------------------------
[Table("nhansu")]
public class NhanSu
{
    [Key]
    [Column("taiKhoanId")]
    public int TaiKhoanId { get; set; }

    [Column("maNhanVien")]
    [MaxLength(20)]
    [Display(Name = "Mã nhân viên")]
    public string? MaNhanVien { get; set; }

    [Column("chucVu")]
    [MaxLength(50)]
    [Display(Name = "Chức vụ")]
    public string? ChucVu { get; set; }

    [Column("luongCoBan")]
    [Display(Name = "Lương cơ bản")]
    public decimal? LuongCoBan { get; set; }

    [Column("ngayVaoLam")]
    [Display(Name = "Ngày vào làm")]
    public DateOnly? NgayVaoLam { get; set; }

    [Column("chuyenMon")]
    [MaxLength(255)]
    [Display(Name = "Chuyên môn")]
    public string? ChuyenMon { get; set; }

    [Column("bangCap")]
    [MaxLength(255)]
    [Display(Name = "Bằng cấp")]
    public string? BangCap { get; set; }

    [Column("hocVi")]
    [MaxLength(100)]
    [Display(Name = "Học vị")]
    public string? HocVi { get; set; }

    [Column("coSoId")]
    [Display(Name = "Cơ sở")]
    public int? CoSoId { get; set; }

    [Column("loaiHopDong")]
    [MaxLength(255)]
    [Display(Name = "Loại hợp đồng")]
    public string? LoaiHopDong { get; set; }

    // 0: Đang làm | 1: Tạm nghỉ | 2: Đã nghỉ việc
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public byte? TrangThai { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [ForeignKey(nameof(CoSoId))]
    public CoSoDaoTao? CoSo { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: nhomquyen — Định nghĩa nhóm quyền (VD: Kế toán, Giáo viên...)
// ---------------------------------------------------------------------------
[Table("nhomquyen")]
public class NhomQuyen
{
    [Key]
    [Column("nhomQuyenId")]
    public int NhomQuyenId { get; set; }

    [Required]
    [Column("tenNhom")]
    [MaxLength(100)]
    [Display(Name = "Tên nhóm")]
    public string TenNhom { get; set; } = string.Empty;

    [Column("moTa")]
    [MaxLength(255)]
    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    public ICollection<PhanQuyen> PhanQuyens { get; set; } = [];
    public ICollection<TaiKhoan> TaiKhoans { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: phanquyen — Chi tiết quyền của từng nhóm trên từng tính năng
// Tính năng: khoa_hoc | lop_hoc | hoc_vien | giao_vien | nhan_vien | tai_chinh...
// ---------------------------------------------------------------------------
[Table("phanquyen")]
public class PhanQuyen
{
    [Key]
    [Column("phanQuyenId")]
    public int PhanQuyenId { get; set; }

    [Column("nhomQuyenId")]
    public int NhomQuyenId { get; set; }

    [Column("tinhNang")]
    [MaxLength(50)]
    [Display(Name = "Tính năng")]
    public string TinhNang { get; set; } = string.Empty;

    [Column("coXem")]
    [Display(Name = "Xem")]
    public bool CoXem { get; set; }

    [Column("coThem")]
    [Display(Name = "Thêm")]
    public bool CoThem { get; set; }

    [Column("coSua")]
    [Display(Name = "Sửa")]
    public bool CoSua { get; set; }

    [Column("coXoa")]
    [Display(Name = "Xóa")]
    public bool CoXoa { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(NhomQuyenId))]
    public NhomQuyen? NhomQuyen { get; set; }
}
