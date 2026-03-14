using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using TrungTamNgoaiNgu.Data;
using TrungTamNgoaiNgu.Enums;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Services;

public class CampusService(AppDbContext db) : ICampusService
{
    public async Task<CampusQuanLyThongKe> LayThongKeAsync()
    {
        return new CampusQuanLyThongKe
        {
            TongCoSo = await db.CoSoDaoTaos.CountAsync(),
            CoSoHoatDong = await db.CoSoDaoTaos.CountAsync(c => c.TrangThai != 0),
            CoSoTamNgung = await db.CoSoDaoTaos.CountAsync(c => c.TrangThai == 0),
            TongPhongHoc = await db.PhongHocs.CountAsync(p => p.DeletedAt == null),
            CoSoDangVanHanh = await db.CoSoDaoTaos.CountAsync(c =>
                db.LopHocs.Any(l =>
                    l.CoSoId == c.CoSoId
                    && l.DeletedAt == null
                    && (l.TrangThai == LopHocTrangThai.DangTuyenSinh
                        || l.TrangThai == LopHocTrangThai.ChotDanhSach
                        || l.TrangThai == LopHocTrangThai.DangHoc))),
            CoSoChuaCoPhong = await db.CoSoDaoTaos.CountAsync(c =>
                !db.PhongHocs.Any(p => p.CoSoId == c.CoSoId && p.DeletedAt == null))
        };
    }

    public async Task<List<CoSoDaoTao>> LayDanhSachAsync(string? tuKhoa = null, int? tinhThanhId = null, int? trangThai = null)
    {
        var query = db.CoSoDaoTaos
            .AsNoTracking()
            .Include(c => c.TinhThanh)
            .Include(c => c.PhongHocs.Where(p => p.DeletedAt == null))
            .Include(c => c.LopHocs.Where(l => l.DeletedAt == null))
            .Where(c => true);

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var keyword = tuKhoa.Trim();
            query = query.Where(c =>
                c.TenCoSo.Contains(keyword)
                || c.MaCoSo.Contains(keyword)
                || (c.TenPhuongXa != null && c.TenPhuongXa.Contains(keyword))
                || (c.DiaChi != null && c.DiaChi.Contains(keyword)));
        }

        if (tinhThanhId.HasValue && tinhThanhId > 0)
            query = query.Where(c => c.TinhThanhId == tinhThanhId.Value);

        if (trangThai.HasValue)
            query = trangThai.Value == 0
                ? query.Where(c => c.TrangThai == 0)
                : query.Where(c => c.TrangThai != 0);

        return await query
            .OrderByDescending(c => c.TrangThai != 0)
            .ThenBy(c => c.TenCoSo)
            .ToListAsync();
    }

    public Task<CoSoDaoTao?> LayTheoIdAsync(int id)
        => db.CoSoDaoTaos
            .AsNoTracking()
            .Include(c => c.TinhThanh)
            .FirstOrDefaultAsync(c => c.CoSoId == id);

    public Task<TinhThanh?> LayTinhThanhTheoIdAsync(int id)
        => db.TinhThanhs
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TinhThanhId == id);

    public async Task<ServiceResult<int>> ThemAsync(CoSoDaoTao coSo, string? nguoiThucHien = null)
    {
        var validation = await KiemTraVaChuanHoaCoSoAsync(coSo);
        if (!validation.ThanhCong)
            return new ServiceResult<int> { ThanhCong = false, ThongBao = validation.ThongBao };

        coSo.MaCoSo = await TaoMaCoSoAsync();
        coSo.Slug = await TaoSlugAsync(coSo.TenCoSo);
        coSo.CreatedAt = DateTime.Now;
        coSo.UpdatedAt = DateTime.Now;
        coSo.TrangThai = ChuanHoaTrangThaiByte(coSo.TrangThai);

        db.CoSoDaoTaos.Add(coSo);
        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Cơ sở", "Tạo cơ sở", coSo.TenCoSo, nguoiThucHien);

        return new ServiceResult<int>
        {
            ThanhCong = true,
            ThongBao = "Đã thêm cơ sở đào tạo.",
            DuLieu = coSo.CoSoId
        };
    }

    public async Task<ServiceResult> CapNhatAsync(CoSoDaoTao coSo, string? nguoiThucHien = null)
    {
        var existing = await db.CoSoDaoTaos.FirstOrDefaultAsync(c => c.CoSoId == coSo.CoSoId);
        if (existing == null)
            return ThatBai("Không tìm thấy cơ sở đào tạo.");

        var validation = await KiemTraVaChuanHoaCoSoAsync(coSo, coSo.CoSoId);
        if (!validation.ThanhCong) return validation;

        existing.TenCoSo = coSo.TenCoSo.Trim();
        existing.Slug = await TaoSlugAsync(existing.TenCoSo, existing.CoSoId);
        existing.DiaChi = coSo.DiaChi?.Trim();
        existing.SoDienThoai = coSo.SoDienThoai?.Trim();
        existing.Email = coSo.Email?.Trim();
        existing.BanDoGoogle = coSo.BanDoGoogle?.Trim();
        existing.TinhThanhId = coSo.TinhThanhId;
        existing.TenPhuongXa = coSo.TenPhuongXa?.Trim();
        existing.ViDo = coSo.ViDo;
        existing.KinhDo = coSo.KinhDo;
        existing.NgayKhaiTruong = coSo.NgayKhaiTruong;
        existing.TrangThai = ChuanHoaTrangThaiByte(coSo.TrangThai);
        existing.UpdatedAt = DateTime.Now;

        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Cơ sở", "Cập nhật cơ sở", existing.TenCoSo, nguoiThucHien);
        return ThanhCong("Đã cập nhật cơ sở đào tạo.");
    }

    public async Task<ServiceResult> XoaAsync(int id, string? nguoiThucHien = null)
    {
        var item = await db.CoSoDaoTaos.FirstOrDefaultAsync(c => c.CoSoId == id);
        if (item == null) return ThatBai("Không tìm thấy cơ sở đào tạo.");

        var dangDung = await db.PhongHocs.AnyAsync(p => p.CoSoId == id && p.DeletedAt == null)
                      || await db.LopHocs.AnyAsync(l => l.CoSoId == id && l.DeletedAt == null)
                      || await db.NhanSus.AnyAsync(n => n.CoSoId == id)
                      || await db.HoaDons.AnyAsync(h => h.CoSoId == id);
        if (dangDung)
            return ThatBai("Không thể xóa cơ sở đang được phòng học, lớp học, nhân sự hoặc hóa đơn sử dụng.");

        db.CoSoDaoTaos.Remove(item);
        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Cơ sở", "Xóa cơ sở", item.TenCoSo, nguoiThucHien);
        return ThanhCong("Đã xóa cơ sở đào tạo.");
    }

    public async Task<CampusTongQuanChiTiet> LayTongQuanChiTietAsync(int coSoId)
    {
        return new CampusTongQuanChiTiet
        {
            TongPhongHoc = await db.PhongHocs.CountAsync(p => p.CoSoId == coSoId && p.DeletedAt == null),
            PhongHoatDong = await db.PhongHocs.CountAsync(p =>
                p.CoSoId == coSoId
                && p.DeletedAt == null
                && p.TrangThai == (int)PhongHocTrangThai.HoatDong),
            TongLopHoc = await db.LopHocs.CountAsync(l => l.CoSoId == coSoId && l.DeletedAt == null),
            LopDangVanHanh = await db.LopHocs.CountAsync(l =>
                l.CoSoId == coSoId
                && l.DeletedAt == null
                && (l.TrangThai == LopHocTrangThai.DangTuyenSinh
                    || l.TrangThai == LopHocTrangThai.ChotDanhSach
                    || l.TrangThai == LopHocTrangThai.DangHoc)),
            TongNhanSu = await db.TaiKhoans.CountAsync(t =>
                t.DeletedAt == null
                && t.NhanSu != null
                && t.NhanSu.CoSoId == coSoId),
            TongGiaoVien = await db.TaiKhoans.CountAsync(t =>
                t.Role == 1
                && t.DeletedAt == null
                && t.NhanSu != null
                && t.NhanSu.CoSoId == coSoId)
        };
    }

    public async Task<List<PhongHoc>> LayPhongTheoCoSoAsync(int coSoId)
        => await db.PhongHocs
            .AsNoTracking()
            .Where(p => p.CoSoId == coSoId && p.DeletedAt == null)
            .OrderBy(p => p.TrangThai != (int)PhongHocTrangThai.HoatDong)
            .ThenBy(p => p.TenPhong)
            .ToListAsync();

    public Task<PhongHoc?> LayPhongTheoIdAsync(int id)
        => db.PhongHocs
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PhongHocId == id && p.DeletedAt == null);

    public async Task<ServiceResult> LuuPhongTheoCoSoAsync(int coSoId, PhongHoc phongHoc, string? nguoiThucHien = null)
    {
        var campus = await db.CoSoDaoTaos.AsNoTracking().FirstOrDefaultAsync(c => c.CoSoId == coSoId);
        if (campus == null)
            return ThatBai("Không tìm thấy cơ sở đào tạo.");

        phongHoc.CoSoId = coSoId;
        if (string.IsNullOrWhiteSpace(phongHoc.TenPhong))
            return ThatBai("Tên phòng không được để trống.");
        if (!phongHoc.SucChua.HasValue || phongHoc.SucChua <= 1)
            return ThatBai("Sức chứa phải lớn hơn 1.");

        var tenPhong = phongHoc.TenPhong.Trim();
        var trungTen = await db.PhongHocs.AnyAsync(p =>
            p.PhongHocId != phongHoc.PhongHocId
            && p.DeletedAt == null
            && p.CoSoId == coSoId
            && p.TenPhong != null
            && p.TenPhong.ToLower() == tenPhong.ToLower());
        if (trungTen)
            return ThatBai("Tên phòng đã tồn tại trong cơ sở này.");

        if (phongHoc.PhongHocId == 0)
        {
            phongHoc.TenPhong = tenPhong;
            phongHoc.TrangThai = ChuanHoaTrangThaiInt(phongHoc.TrangThai);
            phongHoc.CreatedAt = DateTime.Now;
            phongHoc.UpdatedAt = DateTime.Now;
            db.PhongHocs.Add(phongHoc);
            await db.SaveChangesAsync();
            await GhiNhatKyAsync("Phòng học", "Tạo phòng học", $"{campus.TenCoSo} - {phongHoc.TenPhong}", nguoiThucHien);
            return ThanhCong("Đã thêm phòng học.");
        }

        var existing = await db.PhongHocs.FirstOrDefaultAsync(p => p.PhongHocId == phongHoc.PhongHocId && p.DeletedAt == null);
        if (existing == null)
            return ThatBai("Không tìm thấy phòng học.");
        if (existing.CoSoId != coSoId)
            return ThatBai("Phòng học này không thuộc cơ sở hiện tại.");

        existing.TenPhong = tenPhong;
        existing.SucChua = phongHoc.SucChua;
        existing.TrangThietBi = phongHoc.TrangThietBi?.Trim();
        existing.MoTa = phongHoc.MoTa?.Trim();
        existing.GhiChuBaoTri = phongHoc.GhiChuBaoTri?.Trim();
        existing.NgayBaoTri = phongHoc.NgayBaoTri;
        existing.TrangThai = ChuanHoaTrangThaiInt(phongHoc.TrangThai);
        existing.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Phòng học", "Cập nhật phòng học", $"{campus.TenCoSo} - {existing.TenPhong}", nguoiThucHien);
        return ThanhCong("Đã cập nhật phòng học.");
    }

    public async Task<ServiceResult> XoaPhongAsync(int coSoId, int phongHocId, string? nguoiThucHien = null)
    {
        var item = await db.PhongHocs.FirstOrDefaultAsync(p => p.PhongHocId == phongHocId && p.DeletedAt == null);
        if (item == null) return ThatBai("Không tìm thấy phòng học.");
        if (item.CoSoId != coSoId) return ThatBai("Phòng học này không thuộc cơ sở hiện tại.");

        var dangDung = await db.LopHocs.AnyAsync(l => l.PhongHocId == phongHocId && l.DeletedAt == null);
        if (dangDung)
            return ThatBai("Không thể xóa phòng học đang được lớp học sử dụng.");

        item.DeletedAt = DateTime.Now;
        item.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Phòng học", "Xóa phòng học", item.TenPhong, nguoiThucHien);
        return ThanhCong("Đã xóa phòng học.");
    }

    public async Task<List<TaiKhoan>> LayNhanSuTheoCoSoAsync(int coSoId)
        => await db.TaiKhoans
            .AsNoTracking()
            .Where(t => t.DeletedAt == null && t.NhanSu != null && t.NhanSu.CoSoId == coSoId && t.Role != 0)
            .Include(t => t.HoSo)
            .Include(t => t.NhanSu)
            .OrderBy(t => t.Role)
            .ThenBy(t => t.HoSo!.HoTen ?? t.TenTaiKhoan)
            .ToListAsync();

    public async Task<List<LopHoc>> LayLopTheoCoSoAsync(int coSoId)
        => await db.LopHocs
            .AsNoTracking()
            .Where(l => l.CoSoId == coSoId && l.DeletedAt == null)
            .Include(l => l.KhoaHoc)
            .Include(l => l.CaHoc)
            .Include(l => l.PhongHoc)
            .Include(l => l.DangKys)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();

    public async Task<List<TinhThanh>> LayTinhThanhAsync()
        => await db.TinhThanhs
            .AsNoTracking()
            .OrderBy(t => t.TenTinhThanh)
            .ToListAsync();

    public async Task<List<string>> LayPhuongXaNoiBoTheoTinhAsync(int? tinhThanhId, string? baoGomPhuongXa = null)
    {
        if (!tinhThanhId.HasValue || tinhThanhId <= 0)
            return string.IsNullOrWhiteSpace(baoGomPhuongXa) ? [] : [baoGomPhuongXa.Trim()];

        var phuongXas = await db.CoSoDaoTaos
            .AsNoTracking()
            .Where(c => c.TinhThanhId == tinhThanhId.Value)
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

    private async Task<ServiceResult> KiemTraVaChuanHoaCoSoAsync(CoSoDaoTao coSo, int? boQuaId = null)
    {
        if (string.IsNullOrWhiteSpace(coSo.TenCoSo))
            return ThatBai("Tên cơ sở không được để trống.");
        if (!coSo.TinhThanhId.HasValue || coSo.TinhThanhId <= 0)
            return ThatBai("Vui lòng chọn tỉnh/thành.");
        if (string.IsNullOrWhiteSpace(coSo.TenPhuongXa))
            return ThatBai("Vui lòng chọn hoặc nhập phường/xã.");

        var tenCoSo = coSo.TenCoSo.Trim();
        var trungTen = await db.CoSoDaoTaos.AnyAsync(c =>
            c.CoSoId != (boQuaId ?? 0)
            && c.TenCoSo.ToLower() == tenCoSo.ToLower());
        if (trungTen)
            return ThatBai("Tên cơ sở đã tồn tại.");

        coSo.TenCoSo = tenCoSo;
        coSo.TenPhuongXa = coSo.TenPhuongXa?.Trim();
        coSo.DiaChi = coSo.DiaChi?.Trim();
        coSo.SoDienThoai = coSo.SoDienThoai?.Trim();
        coSo.Email = coSo.Email?.Trim();
        coSo.BanDoGoogle = coSo.BanDoGoogle?.Trim();
        coSo.TrangThai = ChuanHoaTrangThaiByte(coSo.TrangThai);

        return ThanhCong(string.Empty);
    }

    private async Task<string> TaoMaCoSoAsync()
    {
        var soThuTu = await db.CoSoDaoTaos.CountAsync() + 1;
        string ma;
        do
        {
            ma = $"CS{soThuTu:000}";
            soThuTu++;
        } while (await db.CoSoDaoTaos.AnyAsync(c => c.MaCoSo == ma));

        return ma;
    }

    private async Task<string> TaoSlugAsync(string ten, int? boQuaId = null)
    {
        var baseSlug = BoDauTiengViet(ten).ToLowerInvariant();
        baseSlug = string.Join("-", baseSlug.Split([' '], StringSplitOptions.RemoveEmptyEntries));
        if (string.IsNullOrWhiteSpace(baseSlug))
            baseSlug = "co-so";

        var slug = baseSlug;
        var stt = 1;
        while (await db.CoSoDaoTaos.AnyAsync(c => c.Slug == slug && (!boQuaId.HasValue || c.CoSoId != boQuaId.Value)))
        {
            slug = $"{baseSlug}-{stt++}";
        }

        return slug;
    }

    private static string BoDauTiengViet(string input)
    {
        var normalized = input.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();
        foreach (var chr in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(chr);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(chr switch
                {
                    'đ' => 'd',
                    'Đ' => 'D',
                    _ => chr
                });
            }
        }

        return builder
            .ToString()
            .Normalize(NormalizationForm.FormC)
            .Replace("/", " ")
            .Replace("\\", " ")
            .Replace(",", " ")
            .Replace(".", " ");
    }

    private async Task GhiNhatKyAsync(string module, string hanhDong, string? noiDung, string? nguoiThucHien)
    {
        db.NhatKyHeThongs.Add(new NhatKyHeThong
        {
            Module = module,
            HanhDong = hanhDong,
            NoiDung = noiDung,
            NguoiThucHien = string.IsNullOrWhiteSpace(nguoiThucHien) ? "Quản trị viên" : nguoiThucHien!,
            CreatedAt = DateTime.Now
        });
        await db.SaveChangesAsync();
    }

    private static byte ChuanHoaTrangThaiByte(byte trangThai) => trangThai == 0 ? (byte)0 : (byte)1;
    private static int ChuanHoaTrangThaiInt(int trangThai) => Enum.IsDefined(typeof(PhongHocTrangThai), trangThai)
        ? trangThai
        : (int)PhongHocTrangThai.HoatDong;
    private static ServiceResult ThanhCong(string message) => new() { ThanhCong = true, ThongBao = message };
    private static ServiceResult ThatBai(string message) => new() { ThanhCong = false, ThongBao = message };
}
