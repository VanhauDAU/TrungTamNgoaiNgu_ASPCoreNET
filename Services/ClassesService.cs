// =============================================================================
// CLASSES SERVICE — IMPLEMENTATION
// Quản lý lớp học: CRUD, state-machine, soft delete, dropdowns
// =============================================================================

using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using TrungTamNgoaiNgu.Data;
using TrungTamNgoaiNgu.Enums;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Services;

public class ClassesService(AppDbContext db) : IClassesService
{
    // =========================================================================
    // DANH SÁCH & PHÂN TRANG
    // =========================================================================

    public async Task<PagedResult<LopHoc>> LayDanhSachPhanTrangAsync(
        int? khoaHocId = null,
        int? coSoId    = null,
        int? trangThai = null,
        string? tuKhoa = null,
        int  page      = 1,
        int  pageSize  = 10)
    {
        page     = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

        var query = db.LopHocs
            .AsNoTracking()
            .Where(l => l.DeletedAt == null);

        if (khoaHocId.HasValue)
            query = query.Where(l => l.KhoaHocId == khoaHocId);

        if (coSoId.HasValue)
            query = query.Where(l => l.CoSoId == coSoId);

        if (trangThai.HasValue)
            query = query.Where(l => (int)l.TrangThai == trangThai.Value);

        if (!string.IsNullOrWhiteSpace(tuKhoa))
            query = query.Where(l => (l.TenLopHoc != null && l.TenLopHoc.Contains(tuKhoa))
                                  || (l.MaLopHoc  != null && l.MaLopHoc.Contains(tuKhoa)));

        var total = await query.CountAsync();

        var items = await query
            .Include(l => l.KhoaHoc)
            .Include(l => l.CoSo)
            .Include(l => l.CaHoc)
            .Include(l => l.PhongHoc)
            .Include(l => l.DangKys)
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<LopHoc>
        {
            Items    = items,
            Total    = total,
            Page     = page,
            PageSize = pageSize
        };
    }

    // =========================================================================
    // THỐNG KÊ
    // =========================================================================

    public async Task<LopHocQuanLyThongKe> LayThongKeAsync()
    {
        var groups = await db.LopHocs
            .Where(l => l.DeletedAt == null)
            .GroupBy(l => l.TrangThai)
            .Select(g => new { TrangThai = g.Key, Count = g.Count() })
            .ToListAsync();

        var dict = groups.ToDictionary(g => g.TrangThai, g => g.Count);

        return new LopHocQuanLyThongKe
        {
            TongLopHoc    = dict.Values.Sum(),
            DangTuyenSinh = dict.GetValueOrDefault(LopHocTrangThai.DangTuyenSinh),
            DangHoc       = dict.GetValueOrDefault(LopHocTrangThai.DangHoc),
            DaKetThuc     = dict.GetValueOrDefault(LopHocTrangThai.DaKetThuc),
            DaHuy         = dict.GetValueOrDefault(LopHocTrangThai.DaHuy),
        };
    }

    // =========================================================================
    // CHI TIẾT
    // =========================================================================

    public async Task<LopHoc?> LayTheoIdAsync(int id)
    {
        return await db.LopHocs
            .Include(l => l.KhoaHoc)
            .Include(l => l.CaHoc)
            .Include(l => l.PhongHoc)
            .Include(l => l.CoSo)
                .ThenInclude(c => c!.TinhThanh)
            .FirstOrDefaultAsync(l => l.LopHocId == id && l.DeletedAt == null);
    }

    // =========================================================================
    // SLUG
    // =========================================================================

    public async Task<string> TaoSlugLopHocAsync(string tenLopHoc, int? boQuaId = null)
    {
        var baseSlug = ChuanHoaSlug(tenLopHoc);
        var slug = baseSlug;
        var stt  = 1;

        while (await db.LopHocs.AnyAsync(l =>
                   l.Slug == slug && (!boQuaId.HasValue || l.LopHocId != boQuaId)))
        {
            slug = $"{baseSlug}-{stt++}";
        }

        return slug;
    }

    // =========================================================================
    // CRUD
    // =========================================================================

    public async Task<ServiceResult> ThemAsync(LopHoc lopHoc, string? nguoiThucHien = null)
    {
        var loiNghiepVu = await KiemTraDuLieuLopHocAsync(lopHoc);
        if (loiNghiepVu != null) return loiNghiepVu;

        lopHoc.MaLopHoc = await SinhMaLopHocAsync(lopHoc.KhoaHocId);

        lopHoc.CreatedAt = DateTime.Now;
        lopHoc.UpdatedAt = DateTime.Now;
        lopHoc.TrangThai = LopHocTrangThai.SapMo; // Trạng thái mặc định khi tạo mới

        db.LopHocs.Add(lopHoc);
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Tạo lớp học",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Lớp: {lopHoc.TenLopHoc} | Slug: {lopHoc.Slug}",
            nguoiThucHien);

        return new ServiceResult { ThanhCong = true, ThongBao = "Đã tạo lớp học thành công." };
    }

    public async Task<ServiceResult> CapNhatAsync(LopHoc lopHoc, string? nguoiThucHien = null)
    {
        var existing = await db.LopHocs
            .FirstOrDefaultAsync(l => l.LopHocId == lopHoc.LopHocId && l.DeletedAt == null);

        if (existing == null)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy lớp học." };

        var loiNghiepVu = await KiemTraDuLieuLopHocAsync(lopHoc, existing);
        if (loiNghiepVu != null) return loiNghiepVu;

        // Ghi nhận thay đổi
        var thayDoi = new List<string>();
        if (existing.TenLopHoc != lopHoc.TenLopHoc) thayDoi.Add($"Tên: {existing.TenLopHoc} → {lopHoc.TenLopHoc}");
        if (existing.KhoaHocId != lopHoc.KhoaHocId) thayDoi.Add($"Khóa học ID: {existing.KhoaHocId} → {lopHoc.KhoaHocId}");
        if (existing.CoSoId     != lopHoc.CoSoId)    thayDoi.Add($"Cơ sở ID: {existing.CoSoId} → {lopHoc.CoSoId}");
        if (existing.PhongHocId != lopHoc.PhongHocId) thayDoi.Add($"Phòng học ID: {existing.PhongHocId} → {lopHoc.PhongHocId}");
        if (existing.CaHocId    != lopHoc.CaHocId)   thayDoi.Add($"Ca học ID: {existing.CaHocId} → {lopHoc.CaHocId}");
        if (existing.TaiKhoanId != lopHoc.TaiKhoanId) thayDoi.Add($"Giáo viên ID: {existing.TaiKhoanId} → {lopHoc.TaiKhoanId}");
        if (existing.HocPhiId != lopHoc.HocPhiId) thayDoi.Add($"Học phí ID: {existing.HocPhiId} → {lopHoc.HocPhiId}");
        if (existing.SoBuoiDuKien != lopHoc.SoBuoiDuKien) thayDoi.Add($"Số buổi: {existing.SoBuoiDuKien} → {lopHoc.SoBuoiDuKien}");
        if (existing.SoHocVienToiDa != lopHoc.SoHocVienToiDa) thayDoi.Add($"Sĩ số: {existing.SoHocVienToiDa} → {lopHoc.SoHocVienToiDa}");
        if (existing.NgayBatDau != lopHoc.NgayBatDau || existing.NgayKetThuc != lopHoc.NgayKetThuc)
            thayDoi.Add($"Lịch ngày: {existing.NgayBatDau} → {lopHoc.NgayBatDau}, {existing.NgayKetThuc} → {lopHoc.NgayKetThuc}");
        if (!string.Equals(existing.LichHoc, lopHoc.LichHoc, StringComparison.Ordinal))
            thayDoi.Add($"Lịch học: {existing.LichHoc} → {lopHoc.LichHoc}");

        // Cập nhật field
        existing.TenLopHoc       = lopHoc.TenLopHoc;
        existing.Slug            = lopHoc.Slug;
        existing.KhoaHocId       = lopHoc.KhoaHocId;
        existing.PhongHocId      = lopHoc.PhongHocId;
        existing.TaiKhoanId      = lopHoc.TaiKhoanId;
        existing.HocPhiId        = lopHoc.HocPhiId;
        existing.NgayBatDau      = lopHoc.NgayBatDau;
        existing.NgayKetThuc     = lopHoc.NgayKetThuc;
        existing.SoBuoiDuKien    = lopHoc.SoBuoiDuKien;
        existing.SoHocVienToiDa  = lopHoc.SoHocVienToiDa;
        existing.DonGiaDay       = lopHoc.DonGiaDay;
        existing.LichHoc         = lopHoc.LichHoc;
        existing.CoSoId          = lopHoc.CoSoId;
        existing.CaHocId         = lopHoc.CaHocId;
        existing.UpdatedAt       = DateTime.Now;

        await db.SaveChangesAsync();

        if (thayDoi.Count > 0)
        {
            await GhiNhatKyAsync(
                "Cập nhật lớp học",
                $"Người thực hiện: {Nguoi(nguoiThucHien)} | Lớp: {existing.TenLopHoc} | {string.Join(" ; ", thayDoi)}",
                nguoiThucHien);
        }

        return new ServiceResult { ThanhCong = true, ThongBao = "Đã cập nhật lớp học." };
    }

    // =========================================================================
    // STATE-MACHINE — Chuyển trạng thái hợp lệ
    // =========================================================================
    //  SapMo(0) → DangTuyenSinh(1) | DaHuy(3)
    //  DangTuyenSinh(1) → ChotDanhSach(2) | DaHuy(3)
    //  ChotDanhSach(2) → DangHoc(4) | DaHuy(3)
    //  DangHoc(4) → DaKetThuc(5)
    //  DaHuy(3) | DaKetThuc(5) → (không chuyển)

    private static readonly Dictionary<LopHocTrangThai, LopHocTrangThai[]> BuocChuyenHopLe = new()
    {
        [LopHocTrangThai.SapMo]         = [LopHocTrangThai.DangTuyenSinh, LopHocTrangThai.DaHuy],
        [LopHocTrangThai.DangTuyenSinh] = [LopHocTrangThai.ChotDanhSach,  LopHocTrangThai.DaHuy],
        [LopHocTrangThai.ChotDanhSach]  = [LopHocTrangThai.DangHoc,        LopHocTrangThai.DaHuy],
        [LopHocTrangThai.DangHoc]       = [LopHocTrangThai.DaKetThuc],
        [LopHocTrangThai.DaHuy]         = [],
        [LopHocTrangThai.DaKetThuc]     = [],
    };

    public async Task<ServiceResult> ChuyenTrangThaiAsync(int id, byte trangThaiMoiByte, string? nguoiThucHien = null)
    {
        var lopHoc = await db.LopHocs.FindAsync(id);
        if (lopHoc == null || lopHoc.DeletedAt != null)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy lớp học." };

        var trangThaiMoi = (LopHocTrangThai)trangThaiMoiByte;
        var buocHopLe = BuocChuyenHopLe.GetValueOrDefault(lopHoc.TrangThai, []);

        if (!buocHopLe.Contains(trangThaiMoi))
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao  = $"Không thể chuyển từ \"{lopHoc.TrangThaiText}\" sang \"{trangThaiMoi.GetLabel()}\"."
            };
        }

        var cuTrangThai = lopHoc.TrangThaiText;
        lopHoc.TrangThai = trangThaiMoi;
        lopHoc.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Chuyển trạng thái lớp học",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Lớp: {lopHoc.TenLopHoc} | {cuTrangThai} → {trangThaiMoi.GetLabel()}",
            nguoiThucHien);

        return new ServiceResult { ThanhCong = true, ThongBao = $"Đã chuyển sang \"{trangThaiMoi.GetLabel()}\"." };
    }

    // =========================================================================
    // SOFT DELETE & TRASH
    // =========================================================================

    public async Task<ServiceResult> XoaMemAsync(int id, string? nguoiThucHien = null)
    {
        var lopHoc = await db.LopHocs.FindAsync(id);
        if (lopHoc == null)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy lớp học." };

        if (lopHoc.TrangThai == LopHocTrangThai.DangHoc)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không thể xóa lớp đang học. Hãy kết thúc hoặc hủy lớp trước." };

        lopHoc.DeletedAt = DateTime.Now;
        lopHoc.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Xóa mềm lớp học",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Lớp: {lopHoc.TenLopHoc}",
            nguoiThucHien);

        return new ServiceResult { ThanhCong = true, ThongBao = "Đã chuyển lớp học vào thùng rác." };
    }

    public async Task<List<LopHoc>> LayThuRacAsync()
    {
        return await db.LopHocs
            .AsNoTracking()
            .Include(l => l.KhoaHoc)
            .Include(l => l.CoSo)
            .Where(l => l.DeletedAt != null)
            .OrderByDescending(l => l.DeletedAt)
            .ToListAsync();
    }

    public async Task<ServiceResult> KhoiPhucAsync(int id, string? nguoiThucHien = null)
    {
        var lopHoc = await db.LopHocs.FindAsync(id);
        if (lopHoc == null || lopHoc.DeletedAt == null)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy lớp học trong thùng rác." };

        lopHoc.DeletedAt = null;
        lopHoc.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Khôi phục lớp học",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Lớp: {lopHoc.TenLopHoc}",
            nguoiThucHien);

        return new ServiceResult { ThanhCong = true, ThongBao = "Đã khôi phục lớp học." };
    }

    // =========================================================================
    // DROPDOWNS
    // =========================================================================

    public async Task<List<KhoaHoc>> LayKhoaHocDropdownAsync(int? baoGomKhoaHocId = null)
        => await db.KhoaHocs
            .AsNoTracking()
            .Where(k =>
                (k.DeletedAt == null && k.TrangThai != 0)
                || (baoGomKhoaHocId.HasValue && k.KhoaHocId == baoGomKhoaHocId.Value))
            .OrderBy(k => k.TenKhoaHoc)
            .ToListAsync();

    public async Task<List<CaHoc>> LayCaHocDropdownAsync(int? baoGomCaHocId = null)
        => await db.CaHocs
            .AsNoTracking()
            .Where(c => c.TrangThai != 0 || (baoGomCaHocId.HasValue && c.CaHocId == baoGomCaHocId.Value))
            .OrderBy(c => c.TenCa)
            .ToListAsync();

    public async Task<List<PhongHoc>> LayPhongHocDropdownAsync(int? coSoId = null, int? baoGomPhongHocId = null)
    {
        var query = db.PhongHocs
            .AsNoTracking()
            .Where(p => p.DeletedAt == null)
            .Where(p =>
                p.TrangThai == (int)PhongHocTrangThai.HoatDong
                || (baoGomPhongHocId.HasValue && p.PhongHocId == baoGomPhongHocId.Value));

        if (coSoId.HasValue)
        {
            query = query.Where(p =>
                p.CoSoId == coSoId
                || (baoGomPhongHocId.HasValue && p.PhongHocId == baoGomPhongHocId.Value));
        }

        return await query
            .OrderBy(p => p.TrangThai != (int)PhongHocTrangThai.HoatDong)
            .ThenBy(p => p.TenPhong)
            .ToListAsync();
    }

    public async Task<List<CoSoDaoTao>> LayCoSoDropdownAsync(int? baoGomCoSoId = null)
        => await db.CoSoDaoTaos
            .AsNoTracking()
            .Where(c => c.TrangThai == 1 || (baoGomCoSoId.HasValue && c.CoSoId == baoGomCoSoId.Value))
            .OrderBy(c => c.TenCoSo)
            .ToListAsync();

    public async Task<List<TaiKhoan>> LayGiaoVienDropdownAsync(int? baoGomTaiKhoanId = null)
        => await db.TaiKhoans
            .AsNoTracking()
            .Where(t => t.Role == 1)
            .Where(t =>
                (t.TrangThai == 1 && t.DeletedAt == null)
                || (baoGomTaiKhoanId.HasValue && t.TaiKhoanId == baoGomTaiKhoanId.Value))
            .Include(t => t.HoSo)
            .Include(t => t.NhanSu)
                .ThenInclude(ns => ns!.CoSo)
            .OrderBy(t => t.TenTaiKhoan)
            .ToListAsync();

    public async Task<List<HocPhi>> LayHocPhiDropdownAsync(int? khoaHocId = null, long? baoGomHocPhiId = null)
    {
        if ((!khoaHocId.HasValue || khoaHocId <= 0) && !baoGomHocPhiId.HasValue)
            return [];

        var query = db.HocPhis
            .AsNoTracking()
            .Where(h =>
                (h.TrangThai != 0 && h.KhoaHoc != null && h.KhoaHoc.DeletedAt == null && h.KhoaHoc.TrangThai != 0)
                || (baoGomHocPhiId.HasValue && h.HocPhiId == baoGomHocPhiId.Value));

        if (khoaHocId.HasValue && khoaHocId > 0)
            query = query.Where(h => h.KhoaHocId == khoaHocId);
        else if (baoGomHocPhiId.HasValue)
            query = query.Where(h => h.HocPhiId == baoGomHocPhiId.Value);

        return await query
            .OrderBy(h => h.SoBuoi)
            .ThenByDescending(h => h.CreatedAt)
            .ToListAsync();
    }

    // =========================================================================
    // CHI TIẾT LỚP: HỌC VIÊN & BUỔI HỌC
    // =========================================================================

    public async Task<List<DangKyLopHoc>> LayHocVienTrongLopAsync(int lopHocId)
        => await db.DangKyLopHocs
            .AsNoTracking()
            .Include(dk => dk.TaiKhoan)
                .ThenInclude(t => t!.HoSo)
            .Where(dk => dk.LopHocId == lopHocId)
            .OrderByDescending(dk => dk.NgayDangKy)
            .ToListAsync();

    public async Task<List<BuoiHoc>> LayBuoiHocAsync(int lopHocId)
        => await db.BuoiHocs
            .AsNoTracking()
            .Include(b => b.CaHoc)
            .Include(b => b.PhongHoc)
            .Where(b => b.LopHocId == lopHocId)
            .OrderBy(b => b.NgayHoc)
            .ToListAsync();

    // =========================================================================
    // DROPDOWN ĐỊA CHỈ 3 TẦNG
    // =========================================================================

    public async Task<List<TinhThanh>> LayTinhThanhDropdownAsync(int? baoGomTinhThanhId = null)
    {
        var tinhIds = db.CoSoDaoTaos
            .AsNoTracking()
            .Where(c => c.TrangThai == 1 || (baoGomTinhThanhId.HasValue && c.TinhThanhId == baoGomTinhThanhId.Value))
            .Where(c => c.TinhThanhId.HasValue)
            .Select(c => c.TinhThanhId!.Value)
            .Distinct();

        return await db.TinhThanhs
            .AsNoTracking()
            .Where(t => tinhIds.Contains(t.TinhThanhId) || (baoGomTinhThanhId.HasValue && t.TinhThanhId == baoGomTinhThanhId.Value))
            .OrderBy(t => t.TenTinhThanh)
            .ToListAsync();
    }

    public async Task<List<string>> LayPhuongXaByTinhAsync(int? tinhThanhId, string? baoGomPhuongXa = null)
    {
        if (!tinhThanhId.HasValue || tinhThanhId <= 0)
            return string.IsNullOrWhiteSpace(baoGomPhuongXa) ? [] : [baoGomPhuongXa.Trim()];

        var phuongXas = await db.CoSoDaoTaos
            .AsNoTracking()
            .Where(c => c.TinhThanhId == tinhThanhId.Value && c.TrangThai == 1)
            .Select(c => c.TenPhuongXa)
            .Where(px => px != null && px != "")
            .Distinct()
            .OrderBy(px => px)
            .ToListAsync();

        var ketQua = phuongXas
            .Where(px => !string.IsNullOrWhiteSpace(px))
            .Select(px => px!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(px => px)
            .ToList();

        if (!string.IsNullOrWhiteSpace(baoGomPhuongXa)
            && !ketQua.Contains(baoGomPhuongXa.Trim(), StringComparer.OrdinalIgnoreCase))
        {
            ketQua.Add(baoGomPhuongXa.Trim());
            ketQua = ketQua
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(px => px)
                .ToList();
        }

        return ketQua;
    }

    public async Task<List<CoSoDaoTao>> LayCoSoByTinhAsync(int? tinhThanhId, string? phuongXa = null, int? baoGomCoSoId = null)
    {
        var query = db.CoSoDaoTaos
            .AsNoTracking()
            .Where(c => c.TrangThai == 1 || (baoGomCoSoId.HasValue && c.CoSoId == baoGomCoSoId.Value));

        if (tinhThanhId.HasValue && tinhThanhId > 0)
            query = query.Where(c => c.TinhThanhId == tinhThanhId);

        if (!string.IsNullOrWhiteSpace(phuongXa))
        {
            var phuongXaFilter = phuongXa.Trim();
            query = query.Where(c => c.TenPhuongXa != null && c.TenPhuongXa.Trim() == phuongXaFilter);
        }

        return await query.OrderBy(c => c.TenCoSo).ToListAsync();
    }

    // =========================================================================
    // MÃ LỚP TỰ SINH
    // Format: K{prefix khoaHoc}-YYYYMM-NNN
    // VD: KIELTS-202603-001 | KEN-202603-012 | KLH-202603-001
    // =========================================================================

    public async Task<string> SinhMaLopHocAsync(int? khoaHocId)
    {
        // Lấy prefix từ mã khóa học (hoặc dùng KH nếu không có)
        string prefix = "KLH";
        if (khoaHocId.HasValue)
        {
            var kh = await db.KhoaHocs.AsNoTracking()
                .FirstOrDefaultAsync(k => k.KhoaHocId == khoaHocId);
            if (kh != null)
            {
                // Dùng MaKhoaHoc nếu có, fallback sang 3 ký tự đầu tên
                prefix = !string.IsNullOrEmpty(kh.MaKhoaHoc)
                    ? $"K{kh.MaKhoaHoc.ToUpperInvariant().Replace("-","").Replace(" ","")}"
                    : $"K{NormPrefix(kh.TenKhoaHoc)}";
            }
        }

        var thang = DateTime.Now.ToString("yyyyMM");
        var pattern = $"{prefix}-{thang}-";

        // Đếm số lớp đã có trong tháng để tạo số thứ tự
        var count = await db.LopHocs
            .Where(l => l.MaLopHoc != null && l.MaLopHoc.StartsWith(pattern))
            .CountAsync();

        return $"{pattern}{(count + 1):D3}";
    }

    private static string NormPrefix(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "LH";
        // Lấy chữ cái đầu của từng từ, tối đa 4 ký tự
        var words = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var initials = string.Concat(words.Select(w => char.ToUpperInvariant(w[0])));
        return initials.Length > 4 ? initials[..4] : initials;
    }

    // =========================================================================

    // PRIVATE HELPERS
    // =========================================================================

    private static string ChuanHoaSlug(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "lop-hoc";

        var normalized = input.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            var cat = CharUnicodeInfo.GetUnicodeCategory(c);
            if (cat == UnicodeCategory.NonSpacingMark) continue;
            if (c == 'đ') sb.Append('d');
            else if (c is >= 'a' and <= 'z' or >= '0' and <= '9') sb.Append(c);
            else sb.Append('-');
        }

        var slug = Regex.Replace(sb.ToString(), "-{2,}", "-").Trim('-');
        if (slug.Length == 0)  slug = "lop-hoc";
        if (slug.Length > 180) slug = slug[..180].TrimEnd('-');
        return slug.Length == 0 ? "lop-hoc" : slug;
    }

    private static string Nguoi(string? s) =>
        string.IsNullOrWhiteSpace(s) ? "Hệ thống" : s.Trim();

    private async Task<ServiceResult?> KiemTraDuLieuLopHocAsync(LopHoc lopHoc, LopHoc? existing = null)
    {
        lopHoc.TenLopHoc = lopHoc.TenLopHoc?.Trim();
        lopHoc.LichHoc = ChuanHoaLichHoc(lopHoc.LichHoc);

        if (!lopHoc.KhoaHocId.HasValue || lopHoc.KhoaHocId <= 0)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Vui lòng chọn khóa học đang hoạt động cho lớp."
            };
        }

        var khoaHoc = await db.KhoaHocs
            .AsNoTracking()
            .FirstOrDefaultAsync(k =>
                k.KhoaHocId == lopHoc.KhoaHocId
                && k.DeletedAt == null
                && k.TrangThai != 0);

        if (khoaHoc == null)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Khóa học không tồn tại hoặc đang tạm ngưng."
            };
        }

        if (existing != null
            && existing.TrangThai >= LopHocTrangThai.ChotDanhSach
            && existing.KhoaHocId != lopHoc.KhoaHocId)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Không thể đổi khóa học khi lớp đã chốt danh sách hoặc đang vận hành."
            };
        }

        if (lopHoc.CaHocId <= 0)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Vui lòng chọn ca học đang hoạt động."
            };
        }

        var caHoc = await db.CaHocs
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CaHocId == lopHoc.CaHocId);

        if (caHoc == null || caHoc.TrangThai == 0)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Ca học không tồn tại hoặc đang tạm ngưng."
            };
        }

        if (lopHoc.SoBuoiDuKien.HasValue && lopHoc.SoBuoiDuKien <= 0)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Số buổi dự kiến phải lớn hơn 0."
            };
        }

        if (lopHoc.DonGiaDay.HasValue && lopHoc.DonGiaDay < 0)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Đơn giá dạy không được nhỏ hơn 0."
            };
        }

        if (lopHoc.TaiKhoanId.HasValue)
        {
            var giaoVien = await db.TaiKhoans
                .AsNoTracking()
                .Include(t => t.NhanSu)
                .FirstOrDefaultAsync(t => t.TaiKhoanId == lopHoc.TaiKhoanId.Value);

            if (giaoVien == null || giaoVien.Role != 1 || giaoVien.DeletedAt != null || giaoVien.TrangThai != 1)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Giáo viên được chọn không còn ở trạng thái phân công hợp lệ."
                };
            }

            if (giaoVien.NhanSu?.TrangThai == 2)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Giáo viên đã nghỉ việc, vui lòng chọn người khác."
                };
            }
        }

        CoSoDaoTao? coSo = null;
        if (lopHoc.CoSoId.HasValue)
        {
            coSo = await db.CoSoDaoTaos
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CoSoId == lopHoc.CoSoId.Value);

            if (coSo == null || coSo.TrangThai == 0)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Cơ sở đào tạo không tồn tại hoặc đang tạm ngưng."
                };
            }
        }

        if (lopHoc.PhongHocId.HasValue)
        {
            var phongHoc = await db.PhongHocs
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PhongHocId == lopHoc.PhongHocId.Value && p.DeletedAt == null);

            if (phongHoc == null)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Phòng học không tồn tại hoặc đã bị xóa."
                };
            }

            if (!lopHoc.CoSoId.HasValue && phongHoc.CoSoId.HasValue)
            {
                lopHoc.CoSoId = phongHoc.CoSoId;
                if (coSo == null)
                {
                    coSo = await db.CoSoDaoTaos
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CoSoId == lopHoc.CoSoId.Value);
                }
            }

            if (lopHoc.CoSoId.HasValue && phongHoc.CoSoId != lopHoc.CoSoId)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Phòng học phải thuộc đúng cơ sở đã chọn."
                };
            }

            if (phongHoc.TrangThai != (int)PhongHocTrangThai.HoatDong)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = $"Phòng \"{phongHoc.TenPhong}\" đang ở trạng thái {PhongHocTrangThaiText(phongHoc.TrangThai)} nên chưa thể phân công cho lớp."
                };
            }
        }

        if (lopHoc.CoSoId.HasValue && coSo != null && coSo.TrangThai == 0)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Cơ sở đào tạo của phòng học đang tạm ngưng, vui lòng chọn cơ sở hoặc phòng khác."
            };
        }

        if (lopHoc.HocPhiId.HasValue)
        {
            var hocPhi = await db.HocPhis
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.HocPhiId == lopHoc.HocPhiId.Value);

            if (hocPhi == null || hocPhi.TrangThai == 0)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Gói học phí không tồn tại hoặc đang tạm ngưng."
                };
            }

            if (hocPhi.KhoaHocId != lopHoc.KhoaHocId)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Gói học phí phải thuộc đúng khóa học đã chọn."
                };
            }

            if (!lopHoc.SoBuoiDuKien.HasValue || lopHoc.SoBuoiDuKien.Value <= 0)
            {
                lopHoc.SoBuoiDuKien = hocPhi.SoBuoi;
            }
            else if (hocPhi.SoBuoi.HasValue && lopHoc.SoBuoiDuKien.Value != hocPhi.SoBuoi.Value)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = $"Số buổi dự kiến phải khớp gói học phí đã chọn ({hocPhi.SoBuoi} buổi)."
                };
            }
        }

        if (lopHoc.HocPhiId.HasValue)
        {
            if (!lopHoc.NgayBatDau.HasValue)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Vui lòng chọn ngày bắt đầu để hệ thống tự tính ngày kết thúc theo gói học phí."
                };
            }

            if (string.IsNullOrWhiteSpace(lopHoc.LichHoc))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Vui lòng chọn ít nhất một ngày học trong tuần để hệ thống tự tính ngày kết thúc."
                };
            }
        }

        if (lopHoc.SoBuoiDuKien.HasValue
            && lopHoc.SoBuoiDuKien.Value > 0
            && lopHoc.NgayBatDau.HasValue
            && !string.IsNullOrWhiteSpace(lopHoc.LichHoc))
        {
            var ngayKetThuc = TinhNgayKetThucTheoSoBuoi(
                lopHoc.NgayBatDau.Value,
                lopHoc.LichHoc,
                lopHoc.SoBuoiDuKien.Value);

            if (!ngayKetThuc.HasValue)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Lịch học hiện tại không hợp lệ để tính ngày kết thúc. Vui lòng kiểm tra lại ngày bắt đầu và các thứ học."
                };
            }

            lopHoc.NgayKetThuc = ngayKetThuc.Value;
        }
        else if (lopHoc.NgayBatDau.HasValue && lopHoc.NgayKetThuc.HasValue && lopHoc.NgayKetThuc < lopHoc.NgayBatDau)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Ngày kết thúc phải bằng hoặc sau ngày bắt đầu."
            };
        }

        if (!string.IsNullOrWhiteSpace(lopHoc.LichHoc) && lopHoc.NgayBatDau.HasValue && lopHoc.NgayKetThuc.HasValue)
        {
            var soBuoiTheoLich = TinhSoBuoiTheoLich(lopHoc.NgayBatDau.Value, lopHoc.NgayKetThuc.Value, lopHoc.LichHoc);
            if (soBuoiTheoLich <= 0)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Khoảng thời gian và lịch học hiện tại không tạo ra buổi học hợp lệ nào."
                };
            }

            if (!lopHoc.SoBuoiDuKien.HasValue || lopHoc.SoBuoiDuKien.Value <= 0)
            {
                lopHoc.SoBuoiDuKien = soBuoiTheoLich;
            }
            else if (lopHoc.SoBuoiDuKien.Value != soBuoiTheoLich)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = $"Số buổi dự kiến hiện tại ({lopHoc.SoBuoiDuKien.Value}) không khớp với lịch học đã cấu hình ({soBuoiTheoLich} buổi)."
                };
            }
        }

        return await KiemTraSiSoTheoPhongAsync(lopHoc);
    }

    private async Task<ServiceResult?> KiemTraSiSoTheoPhongAsync(LopHoc lopHoc)
    {
        if (!lopHoc.PhongHocId.HasValue || !lopHoc.SoHocVienToiDa.HasValue)
            return null;

        var phong = await db.PhongHocs
            .AsNoTracking()
            .Where(p => p.PhongHocId == lopHoc.PhongHocId && p.DeletedAt == null)
            .Select(p => new { p.TenPhong, p.SucChua })
            .FirstOrDefaultAsync();

        if (phong == null)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Phòng học không tồn tại hoặc đã bị xóa."
            };
        }

        if (!phong.SucChua.HasValue || phong.SucChua.Value <= 0)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = $"Phòng \"{phong.TenPhong}\" chưa cấu hình sức chứa hợp lệ."
            };
        }

        if (lopHoc.SoHocVienToiDa.Value >= phong.SucChua.Value)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = $"Sĩ số lớp ({lopHoc.SoHocVienToiDa}) phải nhỏ hơn sức chứa phòng \"{phong.TenPhong}\" ({phong.SucChua})."
            };
        }

        return null;
    }

    private static string ChuanHoaLichHoc(string? lichHoc)
    {
        if (string.IsNullOrWhiteSpace(lichHoc)) return string.Empty;

        var thuTu = new Dictionary<string, int>
        {
            ["2"] = 0,
            ["3"] = 1,
            ["4"] = 2,
            ["5"] = 3,
            ["6"] = 4,
            ["7"] = 5,
            ["CN"] = 6
        };

        var danhSach = lichHoc
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim().ToUpperInvariant())
            .Distinct()
            .Where(thuTu.ContainsKey)
            .OrderBy(x => thuTu[x]);

        return string.Join(',', danhSach);
    }

    private static int TinhSoBuoiTheoLich(DateOnly ngayBatDau, DateOnly ngayKetThuc, string lichHoc)
    {
        if (ngayKetThuc < ngayBatDau || string.IsNullOrWhiteSpace(lichHoc)) return 0;

        var map = new Dictionary<string, DayOfWeek>
        {
            ["2"] = DayOfWeek.Monday,
            ["3"] = DayOfWeek.Tuesday,
            ["4"] = DayOfWeek.Wednesday,
            ["5"] = DayOfWeek.Thursday,
            ["6"] = DayOfWeek.Friday,
            ["7"] = DayOfWeek.Saturday,
            ["CN"] = DayOfWeek.Sunday
        };

        var ngayHoc = lichHoc
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim().ToUpperInvariant())
            .Where(map.ContainsKey)
            .Select(x => map[x])
            .ToHashSet();

        var count = 0;
        for (var current = ngayBatDau; current <= ngayKetThuc; current = current.AddDays(1))
        {
            if (ngayHoc.Contains(current.DayOfWeek))
                count++;
        }

        return count;
    }

    private static DateOnly? TinhNgayKetThucTheoSoBuoi(DateOnly ngayBatDau, string lichHoc, int soBuoiDuKien)
    {
        if (soBuoiDuKien <= 0 || string.IsNullOrWhiteSpace(lichHoc)) return null;

        var map = new Dictionary<string, DayOfWeek>
        {
            ["2"] = DayOfWeek.Monday,
            ["3"] = DayOfWeek.Tuesday,
            ["4"] = DayOfWeek.Wednesday,
            ["5"] = DayOfWeek.Thursday,
            ["6"] = DayOfWeek.Friday,
            ["7"] = DayOfWeek.Saturday,
            ["CN"] = DayOfWeek.Sunday
        };

        var ngayHoc = lichHoc
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim().ToUpperInvariant())
            .Where(map.ContainsKey)
            .Select(x => map[x])
            .ToHashSet();

        if (ngayHoc.Count == 0) return null;

        var daTinh = 0;
        for (var current = ngayBatDau; daTinh < soBuoiDuKien; current = current.AddDays(1))
        {
            if (!ngayHoc.Contains(current.DayOfWeek)) continue;
            daTinh++;
            if (daTinh == soBuoiDuKien) return current;
        }

        return null;
    }

    private static string PhongHocTrangThaiText(int trangThai)
    {
        return Enum.IsDefined(typeof(PhongHocTrangThai), trangThai)
            ? ((PhongHocTrangThai)trangThai).GetLabel()
            : "Không xác định";
    }

    private async Task GhiNhatKyAsync(string tieuDe, string noiDung, string? nguoiThucHien)
    {
        db.NhatKyHeThongs.Add(new NhatKyHeThong
        {
            Module        = "LopHoc",
            HanhDong      = tieuDe,
            NoiDung       = noiDung,
            NguoiThucHien = Nguoi(nguoiThucHien),
            CreatedAt     = DateTime.Now,
        });
        await db.SaveChangesAsync();
    }
}
