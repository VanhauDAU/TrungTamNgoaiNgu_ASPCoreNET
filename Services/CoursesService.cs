// =============================================================================
// COURSE SERVICE — IMPLEMENTATION
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

public class CoursesService(AppDbContext db) : ICoursesService
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
        {
            // Trạng thái khóa học chỉ còn 2 mức: 1=Đang hoạt động, 0=Tạm ngưng.
            // Dữ liệu legacy có thể còn giá trị 2, quy về nhóm hoạt động.
            query = trangThai.Value == 0
                ? query.Where(kh => kh.TrangThai == 0)
                : query.Where(kh => kh.TrangThai != 0);
        }

        var total = await query.CountAsync();

        var items = await query
            .Include(kh => kh.DanhMuc)
                .ThenInclude(dm => dm!.Parent)
            .Include(kh => kh.HocPhis)
            .Include(kh => kh.LopHocs)
            .OrderByDescending(kh => kh.TrangThai != 0)
            .ThenByDescending(kh => kh.UpdatedAt)
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

    public async Task<string> TaoSlugDanhMucAsync(string tenDanhMuc, int? boQuaDanhMucId = null)
    {
        var baseSlug = ChuanHoaSlug(tenDanhMuc);
        var slug = baseSlug;
        var stt = 1;

        while (await db.DanhMucKhoaHocs.AnyAsync(dm =>
                   dm.Slug == slug && (!boQuaDanhMucId.HasValue || dm.DanhMucId != boQuaDanhMucId)))
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
            DangHoatDong = await db.KhoaHocs.CountAsync(k => k.DeletedAt == null && k.TrangThai != 0),
            TamNgung = await db.KhoaHocs.CountAsync(k => k.DeletedAt == null && k.TrangThai == 0),
            DangVanHanh = await db.KhoaHocs.CountAsync(k =>
                k.DeletedAt == null
                && db.LopHocs.Any(l =>
                    l.KhoaHocId == k.KhoaHocId
                    && (l.TrangThai == LopHocTrangThai.DangTuyenSinh
                        || l.TrangThai == LopHocTrangThai.ChotDanhSach
                        || l.TrangThai == LopHocTrangThai.DangHoc))),
            ChuaCoHocPhi = await db.KhoaHocs.CountAsync(k =>
                k.DeletedAt == null
                && k.TrangThai != 0
                && !db.HocPhis.Any(h => h.KhoaHocId == k.KhoaHocId && h.TrangThai != 0)),
            DaXoaMem = await db.KhoaHocs.CountAsync(k => k.DeletedAt != null)
        };
    }

    public async Task<KhoaHoc?> LayTheoIdAsync(int id)
    {
        return await db.KhoaHocs
            .Include(kh => kh.DanhMuc)
                .ThenInclude(dm => dm!.Parent)
            .Include(kh => kh.HocPhis)
            .Include(kh => kh.LopHocs)
            .FirstOrDefaultAsync(kh => kh.KhoaHocId == id && kh.DeletedAt == null);
    }

    public async Task<ServiceResult<int>> ThemAsync(KhoaHoc khoaHoc, string? nguoiThucHien = null)
    {
        var validation = await KiemTraVaChuanHoaKhoaHocAsync(khoaHoc);
        if (!validation.ThanhCong)
        {
            return new ServiceResult<int> { ThanhCong = false, ThongBao = validation.ThongBao };
        }

        khoaHoc.CreatedAt = DateTime.Now;
        khoaHoc.UpdatedAt = DateTime.Now;
        db.KhoaHocs.Add(khoaHoc);
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Tạo khóa học",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Tên: {khoaHoc.TenKhoaHoc} | Trạng thái: {TrangThaiText(khoaHoc.TrangThai)}",
            nguoiThucHien);

        return new ServiceResult<int>
        {
            ThanhCong = true,
            ThongBao = "Đã thêm khóa học thành công.",
            DuLieu = khoaHoc.KhoaHocId
        };
    }

    public async Task<ServiceResult> CapNhatCoKiemTraAsync(KhoaHoc khoaHoc, string? nguoiThucHien = null)
    {
        var existing = await db.KhoaHocs
            .FirstOrDefaultAsync(k => k.KhoaHocId == khoaHoc.KhoaHocId && k.DeletedAt == null);
        if (existing == null)
        {
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy khóa học." };
        }

        var validation = await KiemTraVaChuanHoaKhoaHocAsync(khoaHoc, khoaHoc.KhoaHocId);
        if (!validation.ThanhCong) return validation;

        if (khoaHoc.TrangThai == 0 && existing.TrangThai != 0)
        {
            if (await CoLopDangVanHanhAsync(existing.KhoaHocId))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Không thể tạm ngưng khóa học vì đang có lớp đang tuyển sinh hoặc đang học. Hãy xử lý lớp trước."
                };
            }
        }

        var thayDoi = new List<string>();
        if (!string.Equals(existing.TenKhoaHoc, khoaHoc.TenKhoaHoc, StringComparison.Ordinal))
            thayDoi.Add($"Tên: \"{existing.TenKhoaHoc}\" -> \"{khoaHoc.TenKhoaHoc}\"");
        if (existing.TrangThai != khoaHoc.TrangThai)
            thayDoi.Add($"Trạng thái: {TrangThaiText(existing.TrangThai)} -> {TrangThaiText(khoaHoc.TrangThai)}");
        if (!string.Equals(existing.MaKhoaHoc, khoaHoc.MaKhoaHoc, StringComparison.Ordinal))
            thayDoi.Add($"Mã: {existing.MaKhoaHoc ?? "—"} -> {khoaHoc.MaKhoaHoc ?? "—"}");
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
        existing.MaKhoaHoc      = khoaHoc.MaKhoaHoc;
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

        if (await CoLopDangVanHanhAsync(id))
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Không thể xóa mềm khóa học vì vẫn còn lớp đang tuyển sinh hoặc đang học."
            };
        }

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
        trangThai = trangThai == 0 ? (byte)0 : (byte)1;

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
                         && (l.TrangThai == LopHocTrangThai.DangTuyenSinh
                             || l.TrangThai == LopHocTrangThai.ChotDanhSach
                             || l.TrangThai == LopHocTrangThai.DangHoc))
                .Select(l => l.KhoaHocId!.Value)
                .Distinct()
                .ToHashSetAsync();
        }

        var categoryIds = khoaHocs
            .Where(k => k.DanhMucId.HasValue)
            .Select(k => k.DanhMucId!.Value)
            .Distinct()
            .ToList();

        var danhMucMap = await db.DanhMucKhoaHocs
            .Where(dm => categoryIds.Contains(dm.DanhMucId))
            .ToDictionaryAsync(dm => dm.DanhMucId);

        var daCapNhat = new List<string>();
        var biChan = new List<string>();
        foreach (var k in khoaHocs)
        {
            if (khoaHocBiChan.Contains(k.KhoaHocId))
            {
                biChan.Add(k.TenKhoaHoc);
                continue;
            }

            if (trangThai != 0
                && (!k.DanhMucId.HasValue
                    || !danhMucMap.TryGetValue(k.DanhMucId.Value, out var danhMuc)
                    || danhMuc.DeletedAt != null
                    || danhMuc.TrangThai == 0))
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
                    ? $"Đã cập nhật {daCapNhat.Count} khóa học. Có {biChan.Count} khóa học bị chặn do còn lớp đang vận hành hoặc danh mục không hợp lệ."
                    : "Không thể cập nhật các khóa học đã chọn vì còn lớp đang vận hành hoặc danh mục không hợp lệ."
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

        var khoaHocDangVanHanh = await db.LopHocs
            .Where(l => l.KhoaHocId.HasValue
                     && idSet.Contains(l.KhoaHocId.Value)
                     && (l.TrangThai == LopHocTrangThai.DangTuyenSinh
                         || l.TrangThai == LopHocTrangThai.ChotDanhSach
                         || l.TrangThai == LopHocTrangThai.DangHoc))
            .Select(l => l.KhoaHocId!.Value)
            .Distinct()
            .ToHashSetAsync();

        var daXoa = new List<KhoaHoc>();
        var biChan = new List<string>();
        foreach (var k in khoaHocs)
        {
            if (khoaHocDangVanHanh.Contains(k.KhoaHocId))
            {
                biChan.Add(k.TenKhoaHoc);
                continue;
            }

            k.DeletedAt = DateTime.Now;
            k.UpdatedAt = DateTime.Now;
            daXoa.Add(k);
        }

        if (daXoa.Count == 0)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Không thể xóa mềm các khóa học đã chọn vì vẫn còn lớp đang tuyển sinh hoặc đang học."
            };
        }

        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Bulk xóa mềm khóa học",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Tổng: {daXoa.Count} | " +
            $"Khóa học: {string.Join(", ", daXoa.Select(k => k.TenKhoaHoc))}",
            nguoiThucHien);

        if (biChan.Count > 0)
        {
            return new ServiceResult
            {
                ThanhCong = true,
                ThongBao = $"Đã chuyển {daXoa.Count} khóa học vào thùng rác. Có {biChan.Count} khóa học bị bỏ qua vì còn lớp đang vận hành."
            };
        }

        return new ServiceResult { ThanhCong = true, ThongBao = $"Đã chuyển {daXoa.Count} khóa học vào thùng rác." };
    }

    public async Task<List<DanhMucKhoaHoc>> LayDanhMucAsync(int? baoGomDanhMucId = null)
    {
        return await db.DanhMucKhoaHocs
            .Include(dm => dm.Parent)
            .Where(dm =>
                dm.DeletedAt == null
                && (dm.TrangThai == 1
                    || (baoGomDanhMucId.HasValue && dm.DanhMucId == baoGomDanhMucId.Value)))
            .OrderBy(dm => dm.ParentId ?? 0)
            .ThenBy(dm => dm.SortOrder)
            .ThenBy(dm => dm.TenDanhMuc)
            .ToListAsync();
    }

    public async Task<List<DanhMucKhoaHoc>> LayDanhSachDanhMucAsync(string? tuKhoa = null)
    {
        var query = db.DanhMucKhoaHocs
            .AsNoTracking()
            .Include(dm => dm.Parent)
            .Include(dm => dm.Children.Where(child => child.DeletedAt == null))
            .Include(dm => dm.KhoaHocs.Where(k => k.DeletedAt == null))
            .Where(dm => dm.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(tuKhoa))
            query = query.Where(dm =>
                dm.TenDanhMuc.Contains(tuKhoa)
                || (dm.MaDanhMuc != null && dm.MaDanhMuc.Contains(tuKhoa))
                || dm.Slug.Contains(tuKhoa));

        return await query
            .OrderBy(dm => dm.ParentId ?? 0)
            .ThenBy(dm => dm.SortOrder)
            .ThenBy(dm => dm.TenDanhMuc)
            .ToListAsync();
    }

    public async Task<DanhMucKhoaHoc?> LayDanhMucTheoIdAsync(int id)
    {
        return await db.DanhMucKhoaHocs
            .Include(dm => dm.Parent)
            .Include(dm => dm.Children.Where(child => child.DeletedAt == null))
            .Include(dm => dm.KhoaHocs.Where(k => k.DeletedAt == null))
            .FirstOrDefaultAsync(dm => dm.DanhMucId == id && dm.DeletedAt == null);
    }

    public async Task<DanhMucKhoaHocQuanLyThongKe> LayThongKeDanhMucAsync()
    {
        return new DanhMucKhoaHocQuanLyThongKe
        {
            TongDanhMuc = await db.DanhMucKhoaHocs.CountAsync(dm => dm.DeletedAt == null),
            DanhMucGoc = await db.DanhMucKhoaHocs.CountAsync(dm => dm.DeletedAt == null && dm.ParentId == null),
            DanhMucCon = await db.DanhMucKhoaHocs.CountAsync(dm => dm.DeletedAt == null && dm.ParentId != null),
            DangHoatDong = await db.DanhMucKhoaHocs.CountAsync(dm => dm.DeletedAt == null && dm.TrangThai != 0),
            TamNgung = await db.DanhMucKhoaHocs.CountAsync(dm => dm.DeletedAt == null && dm.TrangThai == 0)
        };
    }

    public async Task<ServiceResult<int>> ThemDanhMucAsync(DanhMucKhoaHoc danhMuc, string? nguoiThucHien = null)
    {
        var validation = await KiemTraVaChuanHoaDanhMucAsync(danhMuc);
        if (!validation.ThanhCong)
        {
            return new ServiceResult<int> { ThanhCong = false, ThongBao = validation.ThongBao };
        }

        danhMuc.CreatedAt = DateTime.Now;
        danhMuc.UpdatedAt = DateTime.Now;
        db.DanhMucKhoaHocs.Add(danhMuc);
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Tạo danh mục",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Danh mục: {danhMuc.TenDanhMuc} | Slug: {danhMuc.Slug}",
            nguoiThucHien);

        return new ServiceResult<int>
        {
            ThanhCong = true,
            ThongBao = "Đã thêm danh mục thành công.",
            DuLieu = danhMuc.DanhMucId
        };
    }

    public async Task<ServiceResult> CapNhatDanhMucAsync(DanhMucKhoaHoc danhMuc, string? nguoiThucHien = null)
    {
        var existing = await db.DanhMucKhoaHocs
            .FirstOrDefaultAsync(dm => dm.DanhMucId == danhMuc.DanhMucId && dm.DeletedAt == null);
        if (existing == null)
        {
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy danh mục." };
        }

        var validation = await KiemTraVaChuanHoaDanhMucAsync(danhMuc, danhMuc.DanhMucId);
        if (!validation.ThanhCong) return validation;

        if (danhMuc.TrangThai == 0 && existing.TrangThai != 0)
        {
            var coDanhMucConHoatDong = await db.DanhMucKhoaHocs.AnyAsync(dm =>
                dm.ParentId == existing.DanhMucId
                && dm.DeletedAt == null
                && dm.TrangThai != 0);
            if (coDanhMucConHoatDong)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Không thể tạm ngưng danh mục khi vẫn còn danh mục con đang hoạt động."
                };
            }

            var coKhoaHocHoatDong = await db.KhoaHocs.AnyAsync(k =>
                k.DanhMucId == existing.DanhMucId
                && k.DeletedAt == null
                && k.TrangThai != 0);
            if (coKhoaHocHoatDong)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Không thể tạm ngưng danh mục khi vẫn còn khóa học đang hoạt động."
                };
            }
        }

        var thayDoi = new List<string>();
        if (!string.Equals(existing.TenDanhMuc, danhMuc.TenDanhMuc, StringComparison.Ordinal))
            thayDoi.Add($"Tên: \"{existing.TenDanhMuc}\" -> \"{danhMuc.TenDanhMuc}\"");
        if (!string.Equals(existing.MaDanhMuc, danhMuc.MaDanhMuc, StringComparison.Ordinal))
            thayDoi.Add($"Mã: {existing.MaDanhMuc ?? "—"} -> {danhMuc.MaDanhMuc ?? "—"}");
        if (existing.ParentId != danhMuc.ParentId)
            thayDoi.Add($"Danh mục cha: {existing.ParentId?.ToString() ?? "Gốc"} -> {danhMuc.ParentId?.ToString() ?? "Gốc"}");
        if (existing.SortOrder != danhMuc.SortOrder)
            thayDoi.Add($"Thứ tự: {existing.SortOrder} -> {danhMuc.SortOrder}");
        if (existing.TrangThai != danhMuc.TrangThai)
            thayDoi.Add($"Trạng thái: {TrangThaiText(existing.TrangThai)} -> {TrangThaiText(danhMuc.TrangThai)}");
        if (!string.Equals(existing.Slug, danhMuc.Slug, StringComparison.Ordinal))
            thayDoi.Add($"Slug: {existing.Slug} -> {danhMuc.Slug}");
        if (!string.Equals(existing.MoTa, danhMuc.MoTa, StringComparison.Ordinal))
            thayDoi.Add("Mô tả: đã thay đổi");

        existing.MaDanhMuc  = danhMuc.MaDanhMuc;
        existing.TenDanhMuc = danhMuc.TenDanhMuc;
        existing.Slug       = danhMuc.Slug;
        existing.MoTa       = danhMuc.MoTa;
        existing.ParentId   = danhMuc.ParentId;
        existing.SortOrder  = danhMuc.SortOrder;
        existing.TrangThai  = danhMuc.TrangThai;
        existing.UpdatedAt  = DateTime.Now;

        await db.SaveChangesAsync();

        if (thayDoi.Count > 0)
        {
            await GhiNhatKyAsync(
                "Cập nhật danh mục",
                $"Người thực hiện: {Nguoi(nguoiThucHien)} | Danh mục: {existing.TenDanhMuc} | Nội dung: {string.Join(" ; ", thayDoi)}",
                nguoiThucHien);
        }

        return new ServiceResult { ThanhCong = true, ThongBao = "Đã cập nhật danh mục." };
    }

    public async Task<ServiceResult> XoaMemDanhMucAsync(int id, string? nguoiThucHien = null)
    {
        var danhMuc = await db.DanhMucKhoaHocs.FindAsync(id);
        if (danhMuc == null)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy danh mục." };

        var coDanhMucCon = await db.DanhMucKhoaHocs.AnyAsync(dm => dm.ParentId == id && dm.DeletedAt == null);
        if (coDanhMucCon)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Không thể xóa mềm danh mục vì vẫn còn danh mục con đang sử dụng. Hãy sắp xếp lại cây danh mục trước."
            };
        }

        var conKhoaHocDangDung = await db.KhoaHocs.AnyAsync(k => k.DanhMucId == id && k.DeletedAt == null);
        if (conKhoaHocDangDung)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Không thể xóa mềm danh mục vì vẫn còn khóa học gắn với danh mục này. Hãy chuyển khóa học sang danh mục khác trước."
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

        var validation = await KiemTraDanhMucChoKhoaHocDaXoaAsync(khoaHoc);
        if (!validation.ThanhCong) return validation;

        khoaHoc.DeletedAt = null;
        khoaHoc.Slug = await TaoSlugKhoaHocAsync(khoaHoc.TenKhoaHoc, khoaHoc.KhoaHocId);
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
            .Include(d => d.Parent)
            .Where(d => d.DeletedAt != null)
            .OrderByDescending(d => d.DeletedAt)
            .ToListAsync();
    }

    public async Task<ServiceResult> KhoiPhucDanhMucAsync(int id, string? nguoiThucHien = null)
    {
        var danhMuc = await db.DanhMucKhoaHocs.FindAsync(id);
        if (danhMuc == null || danhMuc.DeletedAt == null)
            return new ServiceResult { ThanhCong = false, ThongBao = "Không tìm thấy danh mục trong thùng rác." };

        if (danhMuc.ParentId.HasValue)
        {
            var parent = await db.DanhMucKhoaHocs
                .AsNoTracking()
                .FirstOrDefaultAsync(dm => dm.DanhMucId == danhMuc.ParentId.Value);
            if (parent == null || parent.DeletedAt != null)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Không thể khôi phục danh mục vì danh mục cha chưa được khôi phục."
                };
            }
        }

        danhMuc.DeletedAt = null;
        danhMuc.Slug = await TaoSlugDanhMucAsync(danhMuc.TenDanhMuc, danhMuc.DanhMucId);
        danhMuc.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();

        await GhiNhatKyAsync(
            "Khôi phục danh mục",
            $"Người thực hiện: {Nguoi(nguoiThucHien)} | Danh mục: {danhMuc.TenDanhMuc}",
            nguoiThucHien);

        return new ServiceResult { ThanhCong = true, ThongBao = "Đã khôi phục danh mục thành công." };
    }

    private async Task<ServiceResult> KiemTraVaChuanHoaKhoaHocAsync(KhoaHoc khoaHoc, int? boQuaKhoaHocId = null)
    {
        khoaHoc.TenKhoaHoc = ChuanHoaText(khoaHoc.TenKhoaHoc) ?? string.Empty;
        khoaHoc.MaKhoaHoc = ChuanHoaMaThucThe(khoaHoc.MaKhoaHoc);
        khoaHoc.MoTa = ChuanHoaText(khoaHoc.MoTa);
        khoaHoc.DoiTuong = ChuanHoaText(khoaHoc.DoiTuong);
        khoaHoc.KetQuaDatDuoc = ChuanHoaText(khoaHoc.KetQuaDatDuoc);
        khoaHoc.YeuCauDauVao = ChuanHoaText(khoaHoc.YeuCauDauVao);
        khoaHoc.AnhKhoaHoc = ChuanHoaText(khoaHoc.AnhKhoaHoc);
        khoaHoc.TrangThai = khoaHoc.TrangThai == 0 ? (byte)0 : (byte)1;

        if (string.IsNullOrWhiteSpace(khoaHoc.TenKhoaHoc))
        {
            return new ServiceResult { ThanhCong = false, ThongBao = "Tên khóa học không được để trống." };
        }

        if (!khoaHoc.DanhMucId.HasValue || khoaHoc.DanhMucId <= 0)
        {
            return new ServiceResult { ThanhCong = false, ThongBao = "Vui lòng chọn danh mục khóa học." };
        }

        var danhMuc = await db.DanhMucKhoaHocs
            .AsNoTracking()
            .FirstOrDefaultAsync(dm => dm.DanhMucId == khoaHoc.DanhMucId.Value && dm.DeletedAt == null);
        if (danhMuc == null)
        {
            return new ServiceResult { ThanhCong = false, ThongBao = "Danh mục khóa học không còn tồn tại." };
        }

        if (khoaHoc.TrangThai != 0 && danhMuc.TrangThai == 0)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Không thể kích hoạt khóa học khi danh mục đang tạm ngưng. Hãy mở lại danh mục trước."
            };
        }

        if (!boQuaKhoaHocId.HasValue || string.IsNullOrWhiteSpace(khoaHoc.MaKhoaHoc))
        {
            khoaHoc.MaKhoaHoc = await TaoMaKhoaHocAsync(khoaHoc, boQuaKhoaHocId, danhMuc);
        }

        if (await db.KhoaHocs.AnyAsync(k =>
                k.MaKhoaHoc == khoaHoc.MaKhoaHoc
                && (!boQuaKhoaHocId.HasValue || k.KhoaHocId != boQuaKhoaHocId.Value)
                && k.DeletedAt == null))
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Mã khóa học tự sinh đang bị trùng. Vui lòng lưu lại để hệ thống tạo mã khác."
            };
        }

        khoaHoc.Slug = await TaoSlugKhoaHocAsync(khoaHoc.TenKhoaHoc, boQuaKhoaHocId);
        return new ServiceResult { ThanhCong = true, ThongBao = string.Empty };
    }

    private async Task<ServiceResult> KiemTraDanhMucChoKhoaHocDaXoaAsync(KhoaHoc khoaHoc)
    {
        if (!khoaHoc.DanhMucId.HasValue || khoaHoc.DanhMucId <= 0)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Không thể khôi phục khóa học vì chưa có danh mục hợp lệ."
            };
        }

        var danhMuc = await db.DanhMucKhoaHocs
            .AsNoTracking()
            .FirstOrDefaultAsync(dm => dm.DanhMucId == khoaHoc.DanhMucId.Value);
        if (danhMuc == null || danhMuc.DeletedAt != null)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Không thể khôi phục khóa học vì danh mục hiện tại không còn tồn tại hoặc đang nằm trong thùng rác."
            };
        }

        if (khoaHoc.TrangThai != 0 && danhMuc.TrangThai == 0)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Không thể khôi phục khóa học ở trạng thái hoạt động khi danh mục đang tạm ngưng."
            };
        }

        return new ServiceResult { ThanhCong = true, ThongBao = string.Empty };
    }

    private async Task<ServiceResult> KiemTraVaChuanHoaDanhMucAsync(DanhMucKhoaHoc danhMuc, int? boQuaDanhMucId = null)
    {
        danhMuc.TenDanhMuc = ChuanHoaText(danhMuc.TenDanhMuc) ?? string.Empty;
        danhMuc.MaDanhMuc = ChuanHoaMaThucThe(danhMuc.MaDanhMuc);
        danhMuc.MoTa = ChuanHoaText(danhMuc.MoTa);
        danhMuc.TrangThai = danhMuc.TrangThai == 0 ? (byte)0 : (byte)1;
        danhMuc.ParentId = danhMuc.ParentId is null or <= 0 ? null : danhMuc.ParentId;

        if (string.IsNullOrWhiteSpace(danhMuc.TenDanhMuc))
        {
            return new ServiceResult { ThanhCong = false, ThongBao = "Tên danh mục không được để trống." };
        }

        if (boQuaDanhMucId.HasValue && danhMuc.ParentId == boQuaDanhMucId.Value)
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Danh mục không thể chọn chính nó làm danh mục cha."
            };
        }

        if (danhMuc.ParentId.HasValue)
        {
            var parent = await db.DanhMucKhoaHocs
                .AsNoTracking()
                .FirstOrDefaultAsync(dm => dm.DanhMucId == danhMuc.ParentId.Value && dm.DeletedAt == null);
            if (parent == null)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Danh mục cha không còn tồn tại."
                };
            }

            if (boQuaDanhMucId.HasValue && await LaDanhMucConCuaAsync(danhMuc.ParentId.Value, boQuaDanhMucId.Value))
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Không thể chọn một danh mục con làm danh mục cha của chính nó."
                };
            }

            if (danhMuc.TrangThai != 0 && parent.TrangThai == 0)
            {
                return new ServiceResult
                {
                    ThanhCong = false,
                    ThongBao = "Không thể bật danh mục con khi danh mục cha đang tạm ngưng."
                };
            }
        }

        if (!boQuaDanhMucId.HasValue || string.IsNullOrWhiteSpace(danhMuc.MaDanhMuc))
        {
            danhMuc.MaDanhMuc = await TaoMaDanhMucAsync(danhMuc, boQuaDanhMucId);
        }

        if (await db.DanhMucKhoaHocs.AnyAsync(dm =>
                dm.MaDanhMuc == danhMuc.MaDanhMuc
                && (!boQuaDanhMucId.HasValue || dm.DanhMucId != boQuaDanhMucId.Value)
                && dm.DeletedAt == null))
        {
            return new ServiceResult
            {
                ThanhCong = false,
                ThongBao = "Mã danh mục tự sinh đang bị trùng. Vui lòng lưu lại để hệ thống tạo mã khác."
            };
        }

        danhMuc.Slug = await TaoSlugDanhMucAsync(danhMuc.TenDanhMuc, boQuaDanhMucId);
        return new ServiceResult { ThanhCong = true, ThongBao = string.Empty };
    }

    private async Task<bool> LaDanhMucConCuaAsync(int danhMucChaCanKiemTra, int danhMucGoc)
    {
        var parentLookup = await db.DanhMucKhoaHocs
            .AsNoTracking()
            .Where(dm => dm.DeletedAt == null)
            .Select(dm => new { dm.DanhMucId, dm.ParentId })
            .ToDictionaryAsync(dm => dm.DanhMucId, dm => dm.ParentId);

        var currentId = danhMucChaCanKiemTra;
        var daDuyet = new HashSet<int>();
        while (daDuyet.Add(currentId) && parentLookup.TryGetValue(currentId, out var parentId) && parentId.HasValue)
        {
            if (parentId.Value == danhMucGoc) return true;
            currentId = parentId.Value;
        }

        return false;
    }

    private async Task<bool> CoLopDangVanHanhAsync(int khoaHocId)
    {
        return await db.LopHocs.AnyAsync(l =>
            l.KhoaHocId == khoaHocId
            && (l.TrangThai == LopHocTrangThai.DangTuyenSinh
                || l.TrangThai == LopHocTrangThai.ChotDanhSach
                || l.TrangThai == LopHocTrangThai.DangHoc));
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

    private static string? ChuanHoaText(string? input)
    {
        var value = input?.Trim();
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private async Task<string> TaoMaDanhMucAsync(DanhMucKhoaHoc danhMuc, int? boQuaDanhMucId = null)
    {
        var prefix = TaoMaToken(danhMuc.TenDanhMuc, "DM", 4);
        return await TaoMaTheoTienToAsync(
            db.DanhMucKhoaHocs
                .AsNoTracking()
                .Where(dm => dm.DeletedAt == null && (!boQuaDanhMucId.HasValue || dm.DanhMucId != boQuaDanhMucId.Value))
                .Select(dm => dm.MaDanhMuc),
            prefix);
    }

    private async Task<string> TaoMaKhoaHocAsync(KhoaHoc khoaHoc, int? boQuaKhoaHocId = null, DanhMucKhoaHoc? danhMuc = null)
    {
        danhMuc ??= await db.DanhMucKhoaHocs
            .AsNoTracking()
            .FirstAsync(dm => dm.DanhMucId == khoaHoc.DanhMucId!.Value);

        var prefixDanhMuc = TaoMaToken(danhMuc.MaDanhMuc ?? danhMuc.TenDanhMuc, "DM", 3);
        var prefixKhoaHoc = TaoMaToken(khoaHoc.TenKhoaHoc, "KH", 3);
        var prefix = $"{prefixDanhMuc}{prefixKhoaHoc}";

        return await TaoMaTheoTienToAsync(
            db.KhoaHocs
                .AsNoTracking()
                .Where(k => k.DeletedAt == null && (!boQuaKhoaHocId.HasValue || k.KhoaHocId != boQuaKhoaHocId.Value))
                .Select(k => k.MaKhoaHoc),
            prefix);
    }

    private static async Task<string> TaoMaTheoTienToAsync(IQueryable<string?> query, string prefix)
    {
        prefix = string.IsNullOrWhiteSpace(prefix) ? "AUTO" : prefix;
        var maDaCo = await query
            .Where(ma => ma != null && ma.StartsWith(prefix + "-"))
            .ToListAsync();

        var stt = 1;
        string maMoi;
        do
        {
            maMoi = $"{prefix}-{stt:D3}";
            stt++;
        }
        while (maDaCo.Contains(maMoi, StringComparer.OrdinalIgnoreCase));

        return maMoi;
    }

    private static string? ChuanHoaMaThucThe(string? input)
    {
        var value = input?
            .Trim()
            .ToUpperInvariant()
            .Normalize(NormalizationForm.FormD);

        if (string.IsNullOrWhiteSpace(value)) return null;

        var sb = new StringBuilder(value.Length);
        foreach (var c in value)
        {
            var cat = CharUnicodeInfo.GetUnicodeCategory(c);
            if (cat == UnicodeCategory.NonSpacingMark) continue;

            if (c == 'Đ') sb.Append('D');
            else if (char.IsLetterOrDigit(c)) sb.Append(c);
            else sb.Append('-');
        }

        value = Regex.Replace(sb.ToString(), "-{2,}", "-").Trim('-');
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private static string TaoMaToken(string? input, string fallback, int maxLength)
    {
        if (maxLength < 2) maxLength = 2;

        var normalized = ChuanHoaSlug(input)
            .ToUpperInvariant()
            .Split('-', StringSplitOptions.RemoveEmptyEntries);

        if (normalized.Length == 0) return fallback[..Math.Min(fallback.Length, maxLength)];

        var letters = string.Concat(normalized.Where(part => part.Length > 0).Select(part => part[0]));
        if (letters.Length >= 2)
            return letters[..Math.Min(letters.Length, maxLength)];

        var compact = string.Concat(normalized).ToUpperInvariant();
        if (compact.Length == 0) compact = fallback;

        return compact[..Math.Min(compact.Length, maxLength)];
    }

    private static string TrangThaiText(byte trangThai) => trangThai switch
    {
        1 => "Đang hoạt động",
        0 => "Tạm ngưng",
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
