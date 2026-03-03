// =============================================================================
// TAI CHINH SERVICE — IMPLEMENTATION
// =============================================================================

using Microsoft.EntityFrameworkCore;
using TrungTamNgoaiNgu.Data;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Services;

public class TaiChinhService(AppDbContext db) : ITaiChinhService
{
    public async Task<List<HoaDon>> LayDanhSachHoaDonAsync(int? trangThai = null, string? tuKhoa = null)
    {
        var query = db.HoaDons
            .Include(hd => hd.TaiKhoan).ThenInclude(tk => tk!.HoSo)
            .Include(hd => hd.DangKyLopHoc).ThenInclude(dk => dk!.LopHoc)
            .AsQueryable();

        if (trangThai.HasValue)
            query = query.Where(hd => hd.TrangThai == trangThai);

        if (!string.IsNullOrWhiteSpace(tuKhoa))
            query = query.Where(hd =>
                hd.MaHoaDon!.Contains(tuKhoa) ||
                hd.TaiKhoan!.HoSo!.HoTen.Contains(tuKhoa));

        return await query.OrderByDescending(hd => hd.CreatedAt).ToListAsync();
    }

    public async Task<HoaDon?> LayHoaDonTheoIdAsync(int id)
    {
        return await db.HoaDons
            .Include(hd => hd.TaiKhoan).ThenInclude(tk => tk!.HoSo)
            .Include(hd => hd.DangKyLopHoc).ThenInclude(dk => dk!.LopHoc)
            .Include(hd => hd.PhieuThus)
            .FirstOrDefaultAsync(hd => hd.HoaDonId == id);
    }

    public async Task<PhieuThu?> ThuTienAsync(int hoaDonId, decimal soTien, byte phuongThuc, string? ghiChu)
    {
        // Lấy hóa đơn cần thu tiền
        var hoaDon = await db.HoaDons.FindAsync(hoaDonId);
        if (hoaDon == null || soTien <= 0) return null;

        // Tạo phiếu thu mới
        var phieuThu = new PhieuThu
        {
            HoaDonId              = hoaDonId,
            SoTien                = soTien,
            PhuongThucThanhToan   = phuongThuc,
            NgayThu               = DateOnly.FromDateTime(DateTime.Now),
            GhiChu                = ghiChu,
            TrangThai             = 1,
            MaPhieuThu            = TaoMaPhieuThu(),
            CreatedAt             = DateTime.Now,
            UpdatedAt             = DateTime.Now
        };

        db.PhieuThus.Add(phieuThu);

        // Cập nhật số tiền đã trả trên hóa đơn
        hoaDon.DaTra = (hoaDon.DaTra ?? 0) + soTien;

        // Tự động cập nhật trạng thái hóa đơn
        var tongPhaiTra = hoaDon.TongTienSauThue > 0 ? hoaDon.TongTienSauThue : hoaDon.TongTien ?? 0;
        hoaDon.TrangThai = hoaDon.DaTra >= tongPhaiTra
            ? (byte)2   // Đã thanh toán đủ
            : (byte)1;  // Thanh toán một phần
        hoaDon.UpdatedAt = DateTime.Now;

        await db.SaveChangesAsync();
        return phieuThu;
    }

    public async Task<decimal> TinhDoanhThuThangAsync(int nam, int thang)
    {
        return await db.PhieuThus
            .Where(pt => pt.TrangThai == 1
                      && pt.NgayThu.HasValue
                      && pt.NgayThu.Value.Year == nam
                      && pt.NgayThu.Value.Month == thang)
            .SumAsync(pt => pt.SoTien ?? 0);
    }

    // Hàm tạo mã phiếu thu tự động: PT-YYYYMM-XXXXXX
    private static string TaoMaPhieuThu()
    {
        var now = DateTime.Now;
        return $"PT-{now:yyyyMM}-{now.Ticks % 1000000:D6}";
    }
}
