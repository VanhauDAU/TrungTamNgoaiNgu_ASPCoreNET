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

        // Ghi nhận thay đổi
        var thayDoi = new List<string>();
        if (existing.TenLopHoc != lopHoc.TenLopHoc) thayDoi.Add($"Tên: {existing.TenLopHoc} → {lopHoc.TenLopHoc}");
        if (existing.KhoaHocId != lopHoc.KhoaHocId) thayDoi.Add($"Khóa học ID: {existing.KhoaHocId} → {lopHoc.KhoaHocId}");
        if (existing.CoSoId     != lopHoc.CoSoId)    thayDoi.Add($"Cơ sở ID: {existing.CoSoId} → {lopHoc.CoSoId}");
        if (existing.PhongHocId != lopHoc.PhongHocId) thayDoi.Add($"Phòng học ID: {existing.PhongHocId} → {lopHoc.PhongHocId}");
        if (existing.CaHocId    != lopHoc.CaHocId)   thayDoi.Add($"Ca học ID: {existing.CaHocId} → {lopHoc.CaHocId}");
        if (existing.TaiKhoanId != lopHoc.TaiKhoanId) thayDoi.Add($"Giáo viên ID: {existing.TaiKhoanId} → {lopHoc.TaiKhoanId}");

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

    public async Task<List<KhoaHoc>> LayKhoaHocDropdownAsync()
        => await db.KhoaHocs
            .AsNoTracking()
            .Where(k => k.DeletedAt == null && k.TrangThai != 0)
            .OrderBy(k => k.TenKhoaHoc)
            .ToListAsync();

    public async Task<List<CaHoc>> LayCaHocDropdownAsync()
        => await db.CaHocs
            .AsNoTracking()
            .OrderBy(c => c.TenCa)
            .ToListAsync();

    public async Task<List<PhongHoc>> LayPhongHocDropdownAsync(int? coSoId = null)
    {
        var query = db.PhongHocs.AsNoTracking().Where(p => p.DeletedAt == null);
        if (coSoId.HasValue) query = query.Where(p => p.CoSoId == coSoId);
        return await query.OrderBy(p => p.TenPhong).ToListAsync();
    }

    public async Task<List<CoSoDaoTao>> LayCoSoDropdownAsync()
        => await db.CoSoDaoTaos
            .AsNoTracking()
            .OrderBy(c => c.TenCoSo)
            .ToListAsync();

    public async Task<List<TaiKhoan>> LayGiaoVienDropdownAsync()
        => await db.TaiKhoans
            .AsNoTracking()
            .Where(t => t.Role == 1 && t.TrangThai == 1 && t.DeletedAt == null) // role=1: giáo viên
            .Include(t => t.HoSo)
            .OrderBy(t => t.TenTaiKhoan)
            .ToListAsync();

    public async Task<List<HocPhi>> LayHocPhiDropdownAsync(int? khoaHocId = null)
    {
        var query = db.HocPhis.AsNoTracking();
        if (khoaHocId.HasValue) query = query.Where(h => h.KhoaHocId == khoaHocId);
        return await query.OrderByDescending(h => h.CreatedAt).ToListAsync();
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

    public async Task<List<TinhThanh>> LayTinhThanhDropdownAsync()
        => await db.TinhThanhs
            .AsNoTracking()
            .OrderBy(t => t.TenTinhThanh)
            .ToListAsync();

    public async Task<List<CoSoDaoTao>> LayCoSoByTinhAsync(int? tinhThanhId)
    {
        var query = db.CoSoDaoTaos
            .AsNoTracking()
            .Where(c => c.TrangThai == 1);

        if (tinhThanhId.HasValue && tinhThanhId > 0)
            query = query.Where(c => c.TinhThanhId == tinhThanhId);

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
