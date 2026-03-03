// =============================================================================
// NHÓM 3: TÀI CHÍNH
// Gồm: Hóa đơn, Phiếu thu, Lương giáo viên, Chi tiết lương
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrungTamNgoaiNgu.Models;

// ---------------------------------------------------------------------------
// BẢNG: hoadon — Hóa đơn khi học viên đăng ký lớp học
// TrangThai: 0=Chưa thanh toán | 1=Thanh toán một phần | 2=Đã thanh toán đủ
// ---------------------------------------------------------------------------
[Table("hoadon")]
public class HoaDon
{
    [Key]
    [Column("hoaDonId")]
    public int HoaDonId { get; set; }

    [Column("maHoaDon")]
    [MaxLength(20)]
    [Display(Name = "Mã hóa đơn")]
    public string? MaHoaDon { get; set; }

    [Column("ngayLap")]
    [Display(Name = "Ngày lập")]
    public DateOnly? NgayLap { get; set; }

    [Column("ngayHetHan")]
    [Display(Name = "Hạn thanh toán")]
    public DateOnly? NgayHetHan { get; set; }

    [Column("tongTien")]
    [Display(Name = "Tổng tiền")]
    public decimal? TongTien { get; set; }

    [Column("giamGia")]
    [Display(Name = "Giảm giá")]
    public decimal GiamGia { get; set; }

    [Column("thue")]
    [Display(Name = "Thuế (%)")]
    public decimal Thue { get; set; }

    [Column("tongTienSauThue")]
    [Display(Name = "Tổng sau thuế")]
    public decimal TongTienSauThue { get; set; }

    [Column("daTra")]
    [Display(Name = "Đã thanh toán")]
    public decimal? DaTra { get; set; }

    // Học viên được lập hóa đơn
    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    // Đăng ký lớp học nào
    [Column("dangKyLopHocId")]
    public int? DangKyLopHocId { get; set; }

    // 1=Tiền mặt | 2=Chuyển khoản | 3=VNPay
    [Column("phuongThucThanhToan")]
    [Display(Name = "Phương thức TT")]
    public int? PhuongThucThanhToan { get; set; }

    // 0=Đăng ký mới | 1=Gia hạn | 2=Khác
    [Column("loaiHoaDon")]
    [Display(Name = "Loại hóa đơn")]
    public byte LoaiHoaDon { get; set; }

    [Column("coSoId")]
    public int? CoSoId { get; set; }

    // 0=Chưa thanh toán | 1=Thanh toán một phần | 2=Đã đủ
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

    [ForeignKey(nameof(DangKyLopHocId))]
    public DangKyLopHoc? DangKyLopHoc { get; set; }

    [ForeignKey(nameof(CoSoId))]
    public CoSoDaoTao? CoSo { get; set; }

    public ICollection<PhieuThu> PhieuThus { get; set; } = [];

    // Số tiền còn nợ
    [NotMapped]
    public decimal ConNo => (TongTienSauThue > 0 ? TongTienSauThue : TongTien ?? 0) - (DaTra ?? 0);

    [NotMapped]
    public string TrangThaiText => TrangThai switch {
        0 => "Chưa thanh toán", 1 => "Thanh toán một phần", 2 => "Đã thanh toán đủ",
        _ => "Không xác định"
    };

    [NotMapped]
    public string PhuongThucText => PhuongThucThanhToan switch {
        1 => "Tiền mặt", 2 => "Chuyển khoản", 3 => "VNPay", _ => "Chưa xác định"
    };
}

// ---------------------------------------------------------------------------
// BẢNG: phieuthu — Từng lần thu tiền (1 hóa đơn có thể có nhiều phiếu thu)
// TrangThai: 0=Hủy | 1=Hợp lệ
// ---------------------------------------------------------------------------
[Table("phieuthu")]
public class PhieuThu
{
    [Key]
    [Column("phieuThuId")]
    public long PhieuThuId { get; set; }

    [Column("maPhieuThu")]
    [MaxLength(20)]
    [Display(Name = "Mã phiếu thu")]
    public string? MaPhieuThu { get; set; }

    [Column("hoaDonId")]
    public int? HoaDonId { get; set; }

    [Column("soTien")]
    [Display(Name = "Số tiền")]
    public decimal? SoTien { get; set; }

    // 1=Tiền mặt | 2=Chuyển khoản | 3=VNPay
    [Column("phuongThucThanhToan")]
    [Display(Name = "Phương thức TT")]
    public byte PhuongThucThanhToan { get; set; } = 1;

    [Column("ngayThu")]
    [Display(Name = "Ngày thu")]
    public DateOnly? NgayThu { get; set; }

    // Nhân viên thu tiền
    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("ghiChu")]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    // 0=Hủy | 1=Hợp lệ
    [Column("trangThai")]
    public byte TrangThai { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(HoaDonId))]
    public HoaDon? HoaDon { get; set; }

    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? NguoiThu { get; set; }
}

// ---------------------------------------------------------------------------
// BẢNG: luong — Bảng lương hàng tháng của giáo viên/nhân viên
// ---------------------------------------------------------------------------
[Table("luong")]
public class Luong
{
    [Key]
    [Column("luongId")]
    public int LuongId { get; set; }

    [Column("taiKhoanId")]
    public int? TaiKhoanId { get; set; }

    [Column("thangLuong")]
    [MaxLength(10)]
    [Display(Name = "Tháng lương")]
    public string? ThangLuong { get; set; } // VD: "2026-03"

    [Column("tongLuongDay")]
    [Display(Name = "Tổng lương đứng lớp")]
    public decimal? TongLuongDay { get; set; }

    [Column("thuong")]
    [Display(Name = "Thưởng")]
    public decimal Thuong { get; set; }

    [Column("phat")]
    [Display(Name = "Phạt / Khấu trừ")]
    public decimal Phat { get; set; }

    [Column("phuCap")]
    [Display(Name = "Phụ cấp")]
    public decimal PhuCap { get; set; }

    [Column("tongTienThucLanh")]
    [Display(Name = "Thực lĩnh")]
    public decimal? TongTienThucLanh { get; set; }

    [Column("tongBuoiDay")]
    [Display(Name = "Tổng buổi dạy")]
    public int? TongBuoiDay { get; set; }

    [Column("ngayThanhToan")]
    [Display(Name = "Ngày thanh toán")]
    public DateOnly? NgayThanhToan { get; set; }

    [Column("trangThai")]
    public byte? TrangThai { get; set; }

    [Column("ghiChu")]
    public string? GhiChu { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }
    public ICollection<LuongChiTiet> ChiTiets { get; set; } = [];
}

// ---------------------------------------------------------------------------
// BẢNG: luongchitiet — Chi tiết từng lớp giáo viên đã dạy trong tháng
// ---------------------------------------------------------------------------
[Table("luongchitiet")]
public class LuongChiTiet
{
    [Key]
    [Column("luongChiTietId")]
    public int LuongChiTietId { get; set; }

    [Column("luongId")]
    public int? LuongId { get; set; }

    [Column("lopHocId")]
    public int? LopHocId { get; set; }

    [Column("soBuoiDay")]
    [Display(Name = "Số buổi dạy")]
    public int? SoBuoiDay { get; set; }

    [Column("donGiaMotBuoi")]
    [Display(Name = "Đơn giá / buổi")]
    public decimal? DonGiaMotBuoi { get; set; }

    [Column("tongTien")]
    [Display(Name = "Thành tiền")]
    public decimal? TongTien { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(LuongId))]
    public Luong? Luong { get; set; }

    [ForeignKey(nameof(LopHocId))]
    public LopHoc? LopHoc { get; set; }
}
