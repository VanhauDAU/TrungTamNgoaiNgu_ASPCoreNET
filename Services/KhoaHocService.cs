// =============================================================================
// KHOA HOC SERVICE — IMPLEMENTATION
// =============================================================================

using Microsoft.EntityFrameworkCore;
using TrungTamNgoaiNgu.Data;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Services;

public class KhoaHocService(AppDbContext db) : IKhoaHocService
{
    public async Task<List<KhoaHoc>> LayDanhSachAsync(string? tuKhoa = null, int? danhMucId = null)
    {
        // Bắt đầu bằng IQueryable — chưa chạy query thật
        // EF Core sẽ dịch ra SQL tương ứng
        var query = db.KhoaHocs
            .Include(kh => kh.DanhMuc)
            .Where(kh => kh.DeletedAt == null);   // Bỏ qua các khóa đã xóa mềm

        // Thêm điều kiện lọc động (nếu có)
        if (!string.IsNullOrWhiteSpace(tuKhoa))
            query = query.Where(kh => kh.TenKhoaHoc.Contains(tuKhoa));

        if (danhMucId.HasValue)
            query = query.Where(kh => kh.DanhMucId == danhMucId);

        // .ToListAsync() → chạy query thật và trả về kết quả
        return await query.OrderByDescending(kh => kh.CreatedAt).ToListAsync();
    }

    public async Task<KhoaHoc?> LayTheoIdAsync(int id)
    {
        return await db.KhoaHocs
            .Include(kh => kh.DanhMuc)
            .Include(kh => kh.HocPhis)
            .Include(kh => kh.LopHocs)
            .FirstOrDefaultAsync(kh => kh.KhoaHocId == id && kh.DeletedAt == null);
    }

    public async Task<int> ThemAsync(KhoaHoc khoaHoc)
    {
        khoaHoc.CreatedAt = DateTime.Now;
        khoaHoc.UpdatedAt = DateTime.Now;
        db.KhoaHocs.Add(khoaHoc);
        await db.SaveChangesAsync();
        return khoaHoc.KhoaHocId; // EF Core tự điền ID sau khi Insert
    }

    public async Task<bool> CapNhatAsync(KhoaHoc khoaHoc)
    {
        var existing = await db.KhoaHocs.FindAsync(khoaHoc.KhoaHocId);
        if (existing == null) return false;

        // Cập nhật từng field thay vì Update toàn bộ entity
        existing.TenKhoaHoc     = khoaHoc.TenKhoaHoc;
        existing.DanhMucId      = khoaHoc.DanhMucId;
        existing.MoTa           = khoaHoc.MoTa;
        existing.AnhKhoaHoc     = khoaHoc.AnhKhoaHoc;
        existing.DoiTuong       = khoaHoc.DoiTuong;
        existing.KetQuaDatDuoc  = khoaHoc.KetQuaDatDuoc;
        existing.YeuCauDauVao   = khoaHoc.YeuCauDauVao;
        existing.TrangThai      = khoaHoc.TrangThai;
        existing.UpdatedAt      = DateTime.Now;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> XoaMemAsync(int id)
    {
        var khoaHoc = await db.KhoaHocs.FindAsync(id);
        if (khoaHoc == null) return false;

        // Soft delete: chỉ ghi thời gian xóa, không xóa khỏi DB
        khoaHoc.DeletedAt = DateTime.Now;
        khoaHoc.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<DanhMucKhoaHoc>> LayDanhMucAsync()
    {
        return await db.DanhMucKhoaHocs
            .Where(dm => dm.TrangThai == 1)
            .OrderBy(dm => dm.TenDanhMuc)
            .ToListAsync();
    }
}
