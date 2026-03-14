// =============================================================================
// FILE: Models/Shared/TrangThaiExtensions.cs
// MỤC ĐÍCH: Extension methods tiện ích để dùng enums trong View & Controller.
//
// CÁCH DÙNG TRONG RAZOR VIEW:
//   @model LopHoc
//   <span class="badge @Model.TrangThai.GetBadgeClass()">
//       @Model.TrangThai.GetLabel()
//   </span>
//
// CÁCH DÙNG TRONG CONTROLLER:
//   if (lopHoc.TrangThai == LopHocTrangThai.DangHoc) { ... }
//   lopHoc.TrangThai = LopHocTrangThai.DaKetThuc;
//
// CÁCH DÙNG KHI QUERY:
//   var lops = db.LopHocs.Where(l => l.TrangThai == LopHocTrangThai.DangHoc);
// =============================================================================

using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TrungTamNgoaiNgu.Enums;

public static class TrangThaiExtensions
{
    // -------------------------------------------------------------------------
    // Lấy nhãn hiển thị từ [Display(Name = "...")] trên enum
    // -------------------------------------------------------------------------
    public static string GetLabel<T>(this T enumValue) where T : Enum
    {
        var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
        var display = member?.GetCustomAttribute<DisplayAttribute>();
        return display?.Name ?? enumValue.ToString();
    }

    // -------------------------------------------------------------------------
    // Trả về CSS class dùng cho Bootstrap badge
    // -------------------------------------------------------------------------

    public static string GetBadgeClass(this LopHocTrangThai trangThai) => trangThai switch
    {
        LopHocTrangThai.SapMo          => "badge text-bg-secondary",
        LopHocTrangThai.DangTuyenSinh  => "badge text-bg-info",
        LopHocTrangThai.ChotDanhSach   => "badge text-bg-warning",
        LopHocTrangThai.DaHuy          => "badge text-bg-danger",
        LopHocTrangThai.DangHoc        => "badge text-bg-success",
        LopHocTrangThai.DaKetThuc      => "badge text-bg-dark",
        _                              => "badge text-bg-light",
    };

    public static string GetBadgeClass(this DangKyTrangThai trangThai) => trangThai switch
    {
        DangKyTrangThai.ChoThanhToan    => "badge text-bg-warning",
        DangKyTrangThai.DaXacNhan      => "badge text-bg-info",
        DangKyTrangThai.DangHoc        => "badge text-bg-success",
        DangKyTrangThai.TamDungNoHocPhi=> "badge text-bg-danger",
        DangKyTrangThai.BaoLuu         => "badge text-bg-secondary",
        DangKyTrangThai.HoanThanh      => "badge text-bg-primary",
        DangKyTrangThai.Huy            => "badge text-bg-dark",
        _                              => "badge text-bg-light",
    };

    public static string GetBadgeClass(this BuoiHocTrangThai trangThai) => trangThai switch
    {
        BuoiHocTrangThai.SapDienRa   => "badge text-bg-secondary",
        BuoiHocTrangThai.DangDienRa  => "badge text-bg-success",
        BuoiHocTrangThai.DaHoanThanh => "badge text-bg-primary",
        BuoiHocTrangThai.DaHuy       => "badge text-bg-danger",
        BuoiHocTrangThai.DoiLich     => "badge text-bg-warning",
        _                            => "badge text-bg-light",
    };

    public static string GetBadgeClass(this DiemDanhTrangThai trangThai) => trangThai switch
    {
        DiemDanhTrangThai.Vang         => "badge text-bg-danger",
        DiemDanhTrangThai.CoMat        => "badge text-bg-success",
        DiemDanhTrangThai.DiTre        => "badge text-bg-warning",
        DiemDanhTrangThai.CoPhep       => "badge text-bg-info",
        DiemDanhTrangThai.KhoaNoHocPhi => "badge text-bg-dark",
        _                              => "badge text-bg-light",
    };

    public static string GetBadgeClass(this PhongHocTrangThai trangThai) => trangThai switch
    {
        PhongHocTrangThai.DangBaoTri => "setup-badge warning",
        PhongHocTrangThai.HoatDong   => "setup-badge active",
        PhongHocTrangThai.TamNgung   => "setup-badge inactive",
        PhongHocTrangThai.NgungHan   => "setup-badge dark",
        _                            => "setup-badge inactive",
    };

    // -------------------------------------------------------------------------
    // Icon Bootstrap Icons tương ứng (dùng trong View)
    // <i class="bi @Model.TrangThai.GetIcon()"></i>
    // -------------------------------------------------------------------------

    public static string GetIcon(this LopHocTrangThai trangThai) => trangThai switch
    {
        LopHocTrangThai.SapMo          => "bi-clock",
        LopHocTrangThai.DangTuyenSinh  => "bi-person-plus",
        LopHocTrangThai.ChotDanhSach   => "bi-check2-circle",
        LopHocTrangThai.DaHuy          => "bi-x-circle",
        LopHocTrangThai.DangHoc        => "bi-mortarboard",
        LopHocTrangThai.DaKetThuc      => "bi-flag-fill",
        _                              => "bi-question-circle",
    };

    public static string GetIcon(this DangKyTrangThai trangThai) => trangThai switch
    {
        DangKyTrangThai.ChoThanhToan    => "bi-hourglass",
        DangKyTrangThai.DaXacNhan      => "bi-check-circle",
        DangKyTrangThai.DangHoc        => "bi-mortarboard",
        DangKyTrangThai.TamDungNoHocPhi=> "bi-lock",
        DangKyTrangThai.BaoLuu         => "bi-archive",
        DangKyTrangThai.HoanThanh      => "bi-trophy",
        DangKyTrangThai.Huy            => "bi-x-circle",
        _                              => "bi-question-circle",
    };

    public static string GetIcon(this BuoiHocTrangThai trangThai) => trangThai switch
    {
        BuoiHocTrangThai.SapDienRa   => "bi-calendar-event",
        BuoiHocTrangThai.DangDienRa  => "bi-play-circle",
        BuoiHocTrangThai.DaHoanThanh => "bi-check-circle-fill",
        BuoiHocTrangThai.DaHuy       => "bi-x-circle",
        BuoiHocTrangThai.DoiLich     => "bi-arrow-repeat",
        _                            => "bi-question-circle",
    };

    public static string GetIcon(this DiemDanhTrangThai trangThai) => trangThai switch
    {
        DiemDanhTrangThai.CoMat        => "bi-person-check",
        DiemDanhTrangThai.Vang         => "bi-person-x",
        DiemDanhTrangThai.DiTre        => "bi-clock-history",
        DiemDanhTrangThai.CoPhep       => "bi-person-exclamation",
        DiemDanhTrangThai.KhoaNoHocPhi => "bi-lock",
        _                              => "bi-question-circle",
    };

    // -------------------------------------------------------------------------
    // Helper: Kiểm tra học viên có được phép điểm danh không
    // -------------------------------------------------------------------------
    public static bool CoTheHoc(this DangKyTrangThai trangThai)
        => trangThai == DangKyTrangThai.DangHoc;

    public static bool BiKhoaDiemDanh(this DangKyTrangThai trangThai)
        => trangThai == DangKyTrangThai.TamDungNoHocPhi;

    // Helper: Lớp còn nhận đăng ký không?
    public static bool DangNhanDangKy(this LopHocTrangThai trangThai)
        => trangThai == LopHocTrangThai.DangTuyenSinh;

    // Helper: Buổi học có thể điểm danh không?
    public static bool CoTheHoc(this BuoiHocTrangThai trangThai)
        => trangThai == BuoiHocTrangThai.SapDienRa
        || trangThai == BuoiHocTrangThai.DangDienRa;
}
