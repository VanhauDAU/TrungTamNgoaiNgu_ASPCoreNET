// =============================================================================
// DASHBOARD SERVICE — IMPLEMENTATION
// =============================================================================
// Implementation = Phần code thực sự thực hiện logic nghiệp vụ
// Controller sẽ gọi interface, .NET tự inject implementation vào
// =============================================================================

using Microsoft.EntityFrameworkCore;
using TrungTamNgoaiNgu.Data;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Services;

public class DashboardService(AppDbContext db) : IDashboardService
{
    public async Task<DashboardThongKe> LayThongKeAsync()
    {
        var thangHienTai = DateTime.Now.Month;
        var namHienTai = DateTime.Now.Year;

        // ⚠️ EF Core DbContext KHÔNG thread-safe
        // → KHÔNG dùng Task.WhenAll, phải await từng query một cách tuần tự
        var soHocVien  = await db.TaiKhoans.CountAsync(tk => tk.Role == 0 && tk.DeletedAt == null);
        var soGiaoVien = await db.TaiKhoans.CountAsync(tk => tk.Role == 1 && tk.DeletedAt == null);
        var soNhanVien = await db.TaiKhoans.CountAsync(tk => tk.Role == 2 && tk.DeletedAt == null);
        var soKhoaHoc  = await db.KhoaHocs.CountAsync(kh => kh.TrangThai == 1 && kh.DeletedAt == null);
        var soLopHoc   = await db.LopHocs.CountAsync(lh => lh.TrangThai == 4 || lh.TrangThai == 1);

        var soDangKy = await db.DangKyLopHocs.CountAsync(
            dk => dk.CreatedAt.Month == thangHienTai && dk.CreatedAt.Year == namHienTai);

        // Tính doanh thu tháng hiện tại
        var doanhThu = await db.PhieuThus
            .Where(pt => pt.TrangThai == 1
                      && pt.NgayThu.HasValue
                      && pt.NgayThu.Value.Month == thangHienTai
                      && pt.NgayThu.Value.Year == namHienTai)
            .SumAsync(pt => pt.SoTien ?? 0);

        // 5 đăng ký mới nhất
        var dangKyMoiNhat = await db.DangKyLopHocs
            .Include(dk => dk.TaiKhoan).ThenInclude(tk => tk!.HoSo)
            .Include(dk => dk.LopHoc)
            .OrderByDescending(dk => dk.CreatedAt)
            .Take(5)
            .ToListAsync();

        // Hóa đơn chưa thanh toán
        var hoaDonChuaTT = await db.HoaDons
            .Include(hd => hd.TaiKhoan).ThenInclude(tk => tk!.HoSo)
            .Where(hd => hd.TrangThai == 0)
            .OrderByDescending(hd => hd.CreatedAt)
            .Take(10)
            .ToListAsync();

        return new DashboardThongKe
        {
            SoHocVien           = soHocVien,
            SoGiaoVien          = soGiaoVien,
            SoNhanVien          = soNhanVien,
            SoKhoaHoc           = soKhoaHoc,
            SoLopHocDangDay     = soLopHoc,
            SoDangKyMoi         = soDangKy,
            DoanhThuThang       = doanhThu,
            DangKyMoiNhat       = dangKyMoiNhat,
            HoaDonChuaThanhToan = hoaDonChuaTT
        };
    }
}
