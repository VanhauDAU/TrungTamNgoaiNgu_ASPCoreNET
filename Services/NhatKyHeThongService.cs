using Microsoft.EntityFrameworkCore;
using TrungTamNgoaiNgu.Data;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Services;

public class NhatKyHeThongService(AppDbContext db) : INhatKyHeThongService
{
    public async Task<PagedResult<NhatKyHeThong>> LayDanhSachPhanTrangAsync(
        string? module = null,
        string? tuKhoa = null,
        int page = 1,
        int pageSize = 20)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var query = db.NhatKyHeThongs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(module))
            query = query.Where(x => x.Module == module);

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            query = query.Where(x =>
                x.HanhDong.Contains(tuKhoa) ||
                x.NguoiThucHien.Contains(tuKhoa) ||
                (x.NoiDung != null && x.NoiDung.Contains(tuKhoa)));
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<NhatKyHeThong>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<List<string>> LayDanhSachModuleAsync()
    {
        return await db.NhatKyHeThongs
            .AsNoTracking()
            .Select(x => x.Module)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();
    }

    public async Task<NhatKyHeThong?> LayTheoIdAsync(long id)
    {
        return await db.NhatKyHeThongs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.NhatKyId == id);
    }
}
