// =============================================================================
// NHÓM 2: KHÓA HỌC & LỚP HỌC
// Gồm: Khóa học, Danh mục, Học phí, Lớp học, Ca học, Phòng học, Buổi học
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrungTamNgoaiNgu.Enums;

namespace TrungTamNgoaiNgu.Models;


// ---------------------------------------------------------------------------
// BẢNG: diemdanh — Điểm danh học viên theo từng buổi
// TrangThai: 0=Vắng | 1=Có mặt | 2=Đến trễ
// ---------------------------------------------------------------------------

[Table("diemdanh")]
public class DiemDanh
{
    [Key]
    [Column("diemDanhId")]
    public long DiemDanhId { get; set; }

    // Học viên nào
    [Column("taiKhoanId")]
    public int TaiKhoanId { get; set; }

    // Buổi học nào
    [Column("buoiHocId")]
    public int BuoiHocId { get; set; }

    // Liên kết đăng ký lớp học (để kiểm tra nợ học phí)
    [Column("dangKyLopHocId")]
    public int? DangKyLopHocId { get; set; }

    // 0: Vắng | 1: Có mặt | 2: Đến trễ | 3: Có phép | 4: Bị khóa (nợ học phí)
    [Column("trangThai")]
    [Display(Name = "Trạng thái")]
    public DiemDanhTrangThai TrangThai { get; set; } = DiemDanhTrangThai.CoMat;

    // 1 nếu có mặt hoặc đi trễ
    [Column("coMat")]
    public byte CoMat { get; set; }

    // Số phút đi trễ (chỉ dùng khi trangThai = DiTre)
    [Column("phutDiTre")]
    public short? PhutDiTre { get; set; }

    // Lý do vắng/trễ/có phép/nợ học phí
    [Column("lyDo")]
    [MaxLength(500)]
    public string? LyDo { get; set; }

    // 0=Trực tiếp, 1=Online
    [Column("hinhThuc")]
    public HinhThucHoc HinhThuc { get; set; } = HinhThucHoc.TrucTiep;

    // Người điểm danh (GV/Admin)
    [Column("nguoiDiemDanhId")]
    public int? NguoiDiemDanhId { get; set; }

    [Column("thoiGianDiemDanh")]
    public DateTime? ThoiGianDiemDanh { get; set; }

    [Column("ghiChu")]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // --- LIÊN KẾT ---
    [ForeignKey(nameof(TaiKhoanId))]
    public TaiKhoan? TaiKhoan { get; set; }

    [ForeignKey(nameof(BuoiHocId))]
    public BuoiHoc? BuoiHoc { get; set; }

    [ForeignKey(nameof(DangKyLopHocId))]
    public DangKyLopHoc? DangKyLopHoc { get; set; }

    [ForeignKey(nameof(NguoiDiemDanhId))]
    public TaiKhoan? NguoiDiemDanh { get; set; }

    [NotMapped]
    public string TrangThaiText => TrangThai.GetLabel();
}
