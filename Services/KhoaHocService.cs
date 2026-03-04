// =============================================================================
// KHOA HOC SERVICE — IMPLEMENTATION
// =============================================================================

using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using TrungTamNgoaiNgu.Data;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Services;

public class KhoaHocService(AppDbContext db) : IKhoaHocService
{
    public async Task<PagedResult<KhoaHoc>> LayDanhSachPhanTrangAsync(
        string? tuKhoa = null,
        int? danhMucId = null,
        int? trangThai = null,
        int page = 1,
        int pageSize = 10)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

        var query = db.KhoaHocs
            .AsNoTracking()
            .Where(kh => kh.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(tuKhoa))
            query = query.Where(kh => kh.TenKhoaHoc.Contains(tuKhoa));

        if (danhMucId.HasValue)
            query = query.Where(kh => kh.DanhMucId == danhMucId);

        if (trangThai.HasValue)
            query = query.Where(kh => kh.TrangThai == (byte)trangThai.Value);

        var total = await query.CountAsync();

        var items = await query
            .Include(kh => kh.DanhMuc)
            .Include(kh => kh.LopHocs)
            .OrderByDescending(kh => kh.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<KhoaHoc>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<string> TaoSlugKhoaHocAsync(string tenKhoaHoc, int? boQuaKhoaHocId = null)
    {
        var baseSlug = ChuanHoaSlug(tenKhoaHoc);
        var slug = baseSlug;
        var stt = 1;

        while (await db.KhoaHocs.AnyAsync(k =>
                   k.Slug == slug && (!boQuaKhoaHocId.HasValue || k.KhoaHocId != boQuaKhoaHocId)))
        {
            slug = $"{baseSlug}-{stt++}";
        }

        return slug;
    }

    public async Task<KhoaHocQuanLyThongKe> LayThongKeQuanLyAsync()
    {
        return new KhoaHocQuanLyThongKe
        {
            TongKhoaHoc = await db.KhoaHocs.CountAsync(k => k.DeletedAt == null),
            DangMo = await db.KhoaHocs.CountAsync(k => k.DeletedAt == null && k.TrangThai == 1),
            SapKhaiGiang = await db.KhoaHocs.CountAsync(k => k.DeletedAt == null && k.TrangThai == 2),
            DaDong = await db.KhoaHocs.CountAsync(k => k.DeletedAt == null && k.TrangThai == 0),
            DaXoaMem = await db.KhoaHocs.CountAsync(k => k.DeletedAt != null)
        };
    }

    public async Task<KhoaHoc?> LayTheoIdAsync(int id)
    {
        return await db.KhoaHocs
            .Include(kh => kh.DanhMuc)
            .Include(kh => kh.HocPhis)
            .Include(kh => kh.LopHocs)
            .FirstOrDefaultAsync(kh => kh.KhoaHocId == id && kh.DeletedAt == null);
    }

    public async Task<int> ThemAsync(KhoaHoc khoaHoc, string? nguoiThucHien = null)
    {
        khoaHoc.CreatedAt = DateTime.Now;
        khoaHoc.UpdatedAt = DateTime.Now;
        db.KhoaHocs.Add(khoaHoc);
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Tạo khóa học",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Tên: {khoaHoc.TenKhoaHoc} | Trạng thái: {TrangThaiText(khoaHoc.TrangThai)}",
            nguoiThucHien);

        return khoaHoc.KhoaHocId; // EF Core tự điền ID sau khi Insert
    }

    public async Task<ServiceResult> CapNhatCoKiemTraAsync(KhoaHoc khoaHoc, string? nguoiThucHien = null)
    {
        var existing = await db.KhoaHocs
            .FirstOrDefaultAsync(k => k.KhoaHocId == khoaHoc.KhoaHocId && k.DeletedAt == null);
        if (existing == null)
        {
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy khóa học." };
        }

        if (khoaHoc.TrangThai == 0 && existing.TrangThai != 0)
        {
            var coLopDangMo = await db.LopHocs.AnyAsync(l =>
                l.KhoaHocId == existing.KhoaHocId && (l.TrangThai == 1 || l.TrangThai == 4));

            if (coLopDangMo)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Không thể đóng khóa học vì đang có lớp đang mở/đang học. Hãy đóng lớp trước."
                };
            }
        }

        var thayDoi = new List<string>();
        if (!string.Equals(existing.TenKhoaHoc, khoaHoc.TenKhoaHoc, StringComparison.Ordinal))
            thayDoi.Add($"Tên: \"{existing.TenKhoaHoc}\" -> \"{khoaHoc.TenKhoaHoc}\"");
        if (existing.TrangThai != khoaHoc.TrangThai)
            thayDoi.Add($"Trạng thái: {TrangThaiText(existing.TrangThai)} -> {TrangThaiText(khoaHoc.TrangThai)}");
        if (!string.Equals(existing.Slug, khoaHoc.Slug, StringComparison.Ordinal))
            thayDoi.Add($"Slug: {existing.Slug} -> {khoaHoc.Slug}");
        if (existing.DanhMucId != khoaHoc.DanhMucId)
            thayDoi.Add($"Danh mụcId: {existing.DanhMucId?.ToString() ?? "null"} -> {khoaHoc.DanhMucId?.ToString() ?? "null"}");
        if (!string.Equals(existing.MoTa, khoaHoc.MoTa, StringComparison.Ordinal))
            thayDoi.Add("Mô tả: đã thay đổi");
        if (!string.Equals(existing.DoiTuong, khoaHoc.DoiTuong, StringComparison.Ordinal))
            thayDoi.Add("Đối tượng: đã thay đổi");
        if (!string.Equals(existing.KetQuaDatDuoc, khoaHoc.KetQuaDatDuoc, StringComparison.Ordinal))
            thayDoi.Add("Kết quả đạt được: đã thay đổi");
        if (!string.Equals(existing.YeuCauDauVao, khoaHoc.YeuCauDauVao, StringComparison.Ordinal))
            thayDoi.Add("Yêu cầu đầu vào: đã thay đổi");
        if (!string.Equals(existing.AnhKhoaHoc, khoaHoc.AnhKhoaHoc, StringComparison.Ordinal))
            thayDoi.Add("Ảnh khóa học: đã thay đổi");

        // Cập nhật từng field thay vì Update toàn bộ entity
        existing.TenKhoaHoc     = khoaHoc.TenKhoaHoc;
        existing.Slug           = khoaHoc.Slug;
        existing.DanhMucId      = khoaHoc.DanhMucId;
        existing.MoTa           = khoaHoc.MoTa;
        existing.AnhKhoaHoc     = khoaHoc.AnhKhoaHoc;
        existing.DoiTuong       = khoaHoc.DoiTuong;
        existing.KetQuaDatDuoc  = khoaHoc.KetQuaDatDuoc;
        existing.YeuCauDauVao   = khoaHoc.YeuCauDauVao;
        existing.TrangThai      = khoaHoc.TrangThai;
        existing.UpdatedAt      = DateTime.Now;

        await db.SaveChangesAsync();

        if (thayDoi.Count > 0)
        {
            await GhiNhatKyAsync(
                "Cập nhật khóa học",
                $"Người thực hiện: {Nguoi(nguoiThucHien)} | Khóa học: {existing.TenKhoaHoc} | " +
                $"Nội dung: {string.Join(" ; ", thayDoi)}",
                nguoiThucHien);
        }

        return new ServiceResult { ThanhCong = true, ThongBao = "Đã cập nhật khóa học." };
    }

    public async Task<ServiceResult> XoaMemAsync(int id, string? nguoiThucHien = null)
    {
        var khoaHoc = await db.KhoaHocs.FindAsync(id);
        if (khoaHoc == null)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy khóa học." };

        // Soft delete: chỉ ghi thời gian xóa, không xóa khỏi DB
        khoaHoc.DeletedAt = DateTime.Now;
        khoaHoc.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Xóa mềm khóa học",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Khóa học: {khoaHoc.TenKhoaHoc}",
            nguoiThucHien);

        return new ServiceResult { ThanhCong = true, ThongBao = "Đã chuyển khóa học vào thùng rác." };
    }

    public async Task<ServiceResult> DoiTrangThaiHangLoatAsync(List<int> ids, byte trangThai, string? nguoiThucHien = null)
    {
        var idSet = ids.Where(i => i > 0).Distinct().ToList();
        if (idSet.Count == 0)
            return new ServiceResult { ThanhCong = false, ThongBao = "Vui lòng chọn ít nhất một khóa học." };

        var khoaHocs = await db.KhoaHocs
            .Where(k => idSet.Contains(k.KhoaHocId) && k.DeletedAt == null)
            .ToListAsync();
        if (khoaHocs.Count == 0)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy khóa học hợp lệ để cập nhật." };

        var khoaHocBiChan = new HashSet<int>();
        if (trangThai == 0)
        {
            khoaHocBiChan = await db.LopHocs
                .Where(l => l.KhoaHocId.HasValue
                         && idSet.Contains(l.KhoaHocId.Value)
                         && (l.TrangThai == 1 || l.TrangThai == 4))
                .Select(l => l.KhoaHocId!.Value)
                .Distinct()
                .ToHashSetAsync();
        }

        var daCapNhat = new List<string>();
        var biChan = new List<string>();
        foreach (var k in khoaHocs)
        {
            if (khoaHocBiChan.Contains(k.KhoaHocId))
            {
                biChan.Add(k.TenKhoaHoc);
                continue;
            }

            if (k.TrangThai == trangThai) continue;
            k.TrangThai = trangThai;
            k.UpdatedAt = DateTime.Now;
            daCapNhat.Add(k.TenKhoaHoc);
        }

        await db.SaveChangesAsync();

        if (daCapNhat.Count > 0)
        {
            await GhiNhatKyAsync(
                "Bulk đổi trạng thái khóa học",
                $"Người thực hiện: {Nguoi(nguoiThucHien)} | Trạng thái mới: {TrangThaiText(trangThai)} | " +
                $"Khóa học: {string.Join(", ", daCapNhat)}",
                nguoiThucHien);
        }

        if (biChan.Count > 0)
        {
            return new ServiceResult
            {
                ThanhCong = daCapNhat.Count > 0,
                ThongBao = daCapNhat.Count > 0
                    ? $"Đã cập nhật {daCapNhat.Count} khóa học. Có {biChan.Count} khóa học không thể đóng vì đang có lớp mở/đang học."
                    : "Không thể đóng các khóa học đã chọn vì đang có lớp mở/đang học."
            };
        }

        return new ServiceResult
        {
            ThanhCong = daCapNhat.Count > 0,
            ThongBao = daCapNhat.Count > 0 ? $"Đã cập nhật trạng thái {daCapNhat.Count} khóa học." : "Không có khóa học nào cần cập nhật."
        };
    }

    public async Task<ServiceResult> XoaMemHangLoatAsync(List<int> ids, string? nguoiThucHien = null)
    {
        var idSet = ids.Where(i => i > 0).Distinct().ToList();
        if (idSet.Count == 0)
            return new ServiceResult { ThanhCong = false, ThongBao = "Vui lòng chọn ít nhất một khóa học." };

        var khoaHocs = await db.KhoaHocs
            .Where(k => idSet.Contains(k.KhoaHocId) && k.DeletedAt == null)
            .ToListAsync();
        if (khoaHocs.Count == 0)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy khóa học hợp lệ để xóa mềm." };

        foreach (var k in khoaHocs)
        {
            k.DeletedAt = DateTime.Now;
            k.UpdatedAt = DateTime.Now;
        }
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Bulk xóa mềm khóa học",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Tổng: {khoaHocs.Count} | " +
            $"Khóa học: {string.Join(", ", khoaHocs.Select(k => k.TenKhoaHoc))}",
            nguoiThucHien);

        return new ServiceResult { ThanhCong = true, ThongBao = $"Đã chuyển {khoaHocs.Count} khóa học vào thùng rác." };
    }

    public async Task<List<DanhMucKhoaHoc>> LayDanhMucAsync()
    {
        return await db.DanhMucKhoaHocs
            .Where(dm => dm.TrangThai == 1 && dm.DeletedAt == null)
            .OrderBy(dm => dm.TenDanhMuc)
            .ToListAsync();
    }

    public async Task<List<DanhMucKhoaHoc>> LayDanhSachDanhMucAsync(string? tuKhoa = null)
    {
        var query = db.DanhMucKhoaHocs
            .Include(dm => dm.KhoaHocs.Where(k => k.DeletedAt == null))
            .Where(dm => dm.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(tuKhoa))
            query = query.Where(dm => dm.TenDanhMuc.Contains(tuKhoa));

        return await query.OrderByDescending(dm => dm.CreatedAt).ToListAsync();
    }

    public async Task<DanhMucKhoaHoc?> LayDanhMucTheoIdAsync(int id)
    {
        return await db.DanhMucKhoaHocs
            .FirstOrDefaultAsync(dm => dm.DanhMucId == id && dm.DeletedAt == null);
    }

    public async Task<int> ThemDanhMucAsync(DanhMucKhoaHoc danhMuc)
    {
        danhMuc.CreatedAt = DateTime.Now;
        danhMuc.UpdatedAt = DateTime.Now;
        db.DanhMucKhoaHocs.Add(danhMuc);
        await db.SaveChangesAsync();
        return danhMuc.DanhMucId;
    }

    public async Task<bool> CapNhatDanhMucAsync(DanhMucKhoaHoc danhMuc)
    {
        var existing = await db.DanhMucKhoaHocs.FindAsync(danhMuc.DanhMucId);
        if (existing == null) return false;

        existing.TenDanhMuc = danhMuc.TenDanhMuc;
        existing.Slug       = danhMuc.Slug;
        existing.MoTa       = danhMuc.MoTa;
        existing.TrangThai  = danhMuc.TrangThai;
        existing.UpdatedAt  = DateTime.Now;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<ServiceResult> XoaMemDanhMucAsync(int id, string? nguoiThucHien = null)
    {
        var danhMuc = await db.DanhMucKhoaHocs.FindAsync(id);
        if (danhMuc == null)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy danh mục." };

        var conKhoaHocDangMo = await db.KhoaHocs.AnyAsync(k =>
            k.DanhMucId == id && k.DeletedAt == null && k.TrangThai == 1);
        if (conKhoaHocDangMo)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Không thể xóa mềm danh mục vì vẫn còn khóa học đang mở. Hãy chuyển khóa học sang danh mục khác hoặc đóng khóa học trước."
            };
        }

        danhMuc.DeletedAt = DateTime.Now;
        danhMuc.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Xóa mềm danh mục",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Danh mục: {danhMuc.TenDanhMuc}",
            nguoiThucHien);

        return new ServiceResult { ThanhCong = true, ThongBao = "Đã chuyển danh mục vào thùng rác." };
    }

    // ── Thùng rác KhoaHoc ──────────────────────────────────────────────
    public async Task<List<KhoaHoc>> LayThuRacAsync()
    {
        return await db.KhoaHocs
            .Include(k => k.DanhMuc)
            .Where(k => k.DeletedAt != null)
            .OrderByDescending(k => k.DeletedAt)
            .ToListAsync();
    }

    public async Task<ServiceResult> KhoiPhucAsync(int id, string? nguoiThucHien = null)
    {
        var khoaHoc = await db.KhoaHocs.FindAsync(id);
        if (khoaHoc == null || khoaHoc.DeletedAt == null)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy khóa học trong thùng rác." };

        khoaHoc.DeletedAt = null;
        khoaHoc.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Khôi phục khóa học",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Khóa học: {khoaHoc.TenKhoaHoc}",
            nguoiThucHien);

        return new ServiceResult { ThanhCong = true, ThongBao = "Đã khôi phục khóa học thành công." };
    }

    public async Task<ServiceResult> KhoiPhucHangLoatAsync(List<int> ids, string? nguoiThucHien = null)
    {
        var idSet = ids.Where(i => i > 0).Distinct().ToList();
        if (idSet.Count == 0)
            return new ServiceResult { ThanhCong = false, ThongBao = "Vui lòng chọn ít nhất một khóa học." };

        var khoaHocs = await db.KhoaHocs
            .Where(k => idSet.Contains(k.KhoaHocId) && k.DeletedAt != null)
            .ToListAsync();
        if (khoaHocs.Count == 0)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy khóa học hợp lệ để khôi phục." };

        foreach (var k in khoaHocs)
        {
            k.DeletedAt = null;
            k.UpdatedAt = DateTime.Now;
        }
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Bulk khôi phục khóa học",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Tổng: {khoaHocs.Count} | " +
            $"Khóa học: {string.Join(", ", khoaHocs.Select(k => k.TenKhoaHoc))}",
            nguoiThucHien);

        return new ServiceResult { ThanhCong = true, ThongBao = $"Đã khôi phục {khoaHocs.Count} khóa học." };
    }

    // ── Thùng rác DanhMuc ──────────────────────────────────────────────
    public async Task<List<DanhMucKhoaHoc>> LayThuRacDanhMucAsync()
    {
        return await db.DanhMucKhoaHocs
            .Where(d => d.DeletedAt != null)
            .OrderByDescending(d => d.DeletedAt)
            .ToListAsync();
    }

    public async Task<ServiceResult> KhoiPhucDanhMucAsync(int id, string? nguoiThucHien = null)
    {
        var danhMuc = await db.DanhMucKhoaHocs.FindAsync(id);
        if (danhMuc == null || danhMuc.DeletedAt == null)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy danh mục trong thùng rác." };

        danhMuc.DeletedAt = null;
        danhMuc.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Khôi phục danh mục",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Danh mục: {danhMuc.TenDanhMuc}",
            nguoiThucHien);

        return new ServiceResult { ThanhCong = true, ThongBao = "Đã khôi phục danh mục thành công." };
    }

    private static string ChuanHoaSlug(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "khoa-hoc";

        var normalized = input.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory == UnicodeCategory.NonSpacingMark) continue;

            if (c == 'đ') sb.Append('d');
            else if (c is >= 'a' and <= 'z' or >= '0' and <= '9') sb.Append(c);
            else sb.Append('-');
        }

        var slug = Regex.Replace(sb.ToString(), "-{2,}", "-").Trim('-');
        if (slug.Length == 0) slug = "khoa-hoc";
        if (slug.Length > 180) slug = slug[..180].TrimEnd('-');

        return slug.Length == 0 ? "khoa-hoc" : slug;
    }

    private static string TrangThaiText(byte trangThai) => trangThai switch
    {
        1 => "Đang mở",
        2 => "Sắp khai giảng",
        0 => "Đã đóng",
        _ => "Không xác định"
    };

    private static string Nguoi(string? nguoiThucHien)
    {
        return string.IsNullOrWhiteSpace(nguoiThucHien) ? "Hệ thống" : nguoiThucHien.Trim();
    }

    private async Task GhiNhatKyAsync(string tieuDe, string noiDung, string? nguoiThucHien)
    {
        db.NhatKyHeThongs.Add(new NhatKyHeThong
        {
            Module = XacDinhModule(tieuDe),
            HanhDong = tieuDe,
            NoiDung = noiDung,
            NguoiThucHien = Nguoi(nguoiThucHien),
            CreatedAt = DateTime.Now,
        });

        await db.SaveChangesAsync();
    }

    private static string XacDinhModule(string? tieuDe)
    {
        if (string.IsNullOrWhiteSpace(tieuDe)) return "HeThong";
        if (tieuDe.Contains("danh mục", StringComparison.OrdinalIgnoreCase)) return "DanhMuc";
        if (tieuDe.Contains("khóa học", StringComparison.OrdinalIgnoreCase)) return "KhoaHoc";
        return "HeThong";
    }
}
