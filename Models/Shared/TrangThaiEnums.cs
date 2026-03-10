// =============================================================================
// FILE: Enums/TrangThai.cs
// MỤC ĐÍCH: Định nghĩa TẤT CẢ enum trạng thái dùng chung trong toàn hệ thống.
//
// QUY ƯỚC:
//  - Dùng enum thay vì magic number (byte/int) trong Model, Controller, View
//  - Cast vào DB: (byte)LopHocTrangThai.SapMo  → lưu số 0
//  - So sánh : lopHoc.TrangThai == LopHocTrangThai.DangHoc
//  - Hiển thị: LopHocTrangThai.DangHoc.GetLabel()  → "Đang học"
//  - CSS badge: LopHocTrangThai.DangHoc.GetBadgeClass() → "badge-success"
// =============================================================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrungTamNgoaiNgu.Enums;

// ─────────────────────────────────────────────────────────────────────────────
// 1. TRẠNG THÁI LỚP HỌC  (bảng: lophoc.trangThai)
// ─────────────────────────────────────────────────────────────────────────────
//
//  Sơ đồ chuyển trạng thái:
//
//  [SapMo] ──────────────────→ [DangTuyenSinh]
//     │                                │
//     │                        [ChotDanhSach]
//     │                                │
//     │                           [DangHoc]
//     │                                │
//     └──────────────────────── [DaKetThuc]
//
//  *.Da_Huy* có thể xảy ra từ bất kỳ trạng thái nào.
//
public enum LopHocTrangThai : byte
{
    /// <summary>0 – Lớp đã được tạo nhưng chưa mở đăng ký.</summary>
    [Display(Name = "Sắp mở")] SapMo = 0,

    /// <summary>1 – Đang nhận đăng ký từ học viên.</summary>
    [Display(Name = "Đang tuyển sinh")] DangTuyenSinh = 1,

    /// <summary>2 – Đã chốt danh sách, không nhận thêm (lớp chưa khai giảng).</summary>
    [Display(Name = "Chốt danh sách")] ChotDanhSach = 2,

    /// <summary>3 – Lớp bị hủy. Học viên đã đăng ký được hoàn/bảo lưu.</summary>
    [Display(Name = "Đã hủy")] DaHuy = 3,

    /// <summary>4 – Lớp đang học (đã khai giảng, chưa kết thúc).</summary>
    [Display(Name = "Đang học")] DangHoc = 4,

    /// <summary>5 – Lớp đã kết thúc toàn bộ buổi học.</summary>
    [Display(Name = "Đã kết thúc")] DaKetThuc = 5,
}

// ─────────────────────────────────────────────────────────────────────────────
// 2. TRẠNG THÁI ĐĂNG KÝ LỚP HỌC  (bảng: dangkylophoc.trangThai)
// ─────────────────────────────────────────────────────────────────────────────
//
//  Sơ đồ chuyển trạng thái:
//
//  [ChoThanhToan]
//       │ (thanh toán đủ học phí)
//       ↓
//  [DaXacNhan]
//       │ (lớp khai giảng)
//       ↓
//  [DangHoc] ──→ [TamDungNoHocPhi]  (nợ học phí)
//       │              │ (thanh toán xong)
//       │              └────────────────┐
//       │                               ↓
//       ├──→ [BaoLuu]         (học viên xin bảo lưu)
//       │
//       ├──→ [HoanThanh]      (lớp kết thúc & HV đủ điều kiện)
//       │
//       └──→ [Huy]            (hủy đăng ký)
//
public enum DangKyTrangThai : byte
{
    /// <summary>0 – Đã tạo đăng ký nhưng chưa thanh toán học phí.</summary>
    [Display(Name = "Chờ thanh toán")] ChoThanhToan = 0,

    /// <summary>1 – Đã thanh toán đủ, đang chờ lớp khai giảng.</summary>
    [Display(Name = "Đã xác nhận")] DaXacNhan = 1,

    /// <summary>2 – Lớp đang học, học viên đang tham gia.</summary>
    [Display(Name = "Đang học")] DangHoc = 2,

    /// <summary>3 – Tạm dừng vì nợ học phí. Học viên bị khóa điểm danh.</summary>
    [Display(Name = "Tạm dừng – nợ học phí")] TamDungNoHocPhi = 3,

    /// <summary>4 – Học viên xin bảo lưu (giữ quyền học sau).</summary>
    [Display(Name = "Bảo lưu")] BaoLuu = 4,

    /// <summary>5 – Hoàn thành khóa học.</summary>
    [Display(Name = "Hoàn thành")] HoanThanh = 5,

    /// <summary>6 – Đã hủy đăng ký.</summary>
    [Display(Name = "Đã hủy")] Huy = 6,
}

// ─────────────────────────────────────────────────────────────────────────────
// 3. TRẠNG THÁI BUỔI HỌC  (bảng: buoihoc.trangThai)
// ─────────────────────────────────────────────────────────────────────────────
//
//  Sơ đồ chuyển trạng thái:
//
//  [SapDienRa]
//       │ (đến ngày/giờ học)
//       ↓
//  [DangDienRa]
//       │ (điểm danh xong + kết thúc)
//       ↓
//  [DaHoanThanh]
//
//  [SapDienRa | DangDienRa] ──→ [DaHuy]   (admin hủy buổi)
//  [SapDienRa]              ──→ [DoiLich]  (dời sang ngày khác)
//
public enum BuoiHocTrangThai : byte
{
    /// <summary>0 – Buổi chưa đến giờ, chưa diễn ra.</summary>
    [Display(Name = "Sắp diễn ra")] SapDienRa = 0,

    /// <summary>1 – Đang trong giờ học.</summary>
    [Display(Name = "Đang diễn ra")] DangDienRa = 1,

    /// <summary>2 – Đã kết thúc, điểm danh xong (daDiemDanh=true, daHoanThanh=true).</summary>
    [Display(Name = "Đã hoàn thành")] DaHoanThanh = 2,

    /// <summary>3 – Buổi học bị hủy (không diễn ra).</summary>
    [Display(Name = "Đã hủy")] DaHuy = 3,

    /// <summary>4 – Đổi lịch sang ngày khác (sẽ có buổi mới thay thế).</summary>
    [Display(Name = "Đổi lịch")] DoiLich = 4,
}

// ─────────────────────────────────────────────────────────────────────────────
// 4. TRẠNG THÁI ĐIỂM DANH  (bảng: diemDanh.trangThai)
// ─────────────────────────────────────────────────────────────────────────────
public enum DiemDanhTrangThai : byte
{
    /// <summary>0 – Vắng không lý do.</summary>
    [Display(Name = "Vắng")] Vang = 0,

    /// <summary>1 – Có mặt đúng giờ (mặc định khi điểm danh).</summary>
    [Display(Name = "Có mặt")] CoMat = 1,

    /// <summary>2 – Đến trễ. Cần điền thêm phutDiTre.</summary>
    [Display(Name = "Đi trễ")] DiTre = 2,

    /// <summary>3 – Nghỉ có phép (báo trước).</summary>
    [Display(Name = "Có phép")] CoPhep = 3,

    /// <summary>4 – Bị khóa điểm danh do nợ học phí (hệ thống tự set).</summary>
    [Display(Name = "Khóa – nợ HP")] KhoaNoHocPhi = 4,
}

// ─────────────────────────────────────────────────────────────────────────────
// 5. HÌNH THỨC HỌC  (bảng: diemDanh.hinhThuc)
// ─────────────────────────────────────────────────────────────────────────────
public enum HinhThucHoc : byte
{
    /// <summary>0 – Học trực tiếp tại lớp.</summary>
    [Display(Name = "Trực tiếp")] TrucTiep = 0,

    /// <summary>1 – Học online qua Zoom/Meet...</summary>
    [Display(Name = "Online")] Online = 1,
}

// ─────────────────────────────────────────────────────────────────────────────
// 6. TRẠNG THÁI PHÒNG HỌC  (bảng: phonghoc.trangThai)
// ─────────────────────────────────────────────────────────────────────────────
public enum PhongHocTrangThai : int
{
    /// <summary>0 – Đang bảo trì, không sử dụng được.</summary>
    [Display(Name = "Đang bảo trì")] DangBaoTri = 0,

    /// <summary>1 – Hoạt động bình thường.</summary>
    [Display(Name = "Hoạt động")] HoatDong = 1,

    /// <summary>2 – Tạm ngưng sử dụng.</summary>
    [Display(Name = "Tạm ngưng")] TamNgung = 2,

    /// <summary>3 – Ngưng hoàn toàn (không còn dùng).</summary>
    [Display(Name = "Ngưng hẳn")] NgungHan = 3,
}
