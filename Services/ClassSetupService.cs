using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using TrungTamNgoaiNgu.Data;
using TrungTamNgoaiNgu.Models;
using TrungTamNgoaiNgu.Services.Interfaces;

namespace TrungTamNgoaiNgu.Services;

public class ClassSetupService(AppDbContext db) : IClassSetupService
{
    public async Task<ClassSetupThongKe> LayThongKeAsync()
    {
        return new ClassSetupThongKe
        {
            TongCaHoc = await db.CaHocs.CountAsync(),
            TongCoSo = await db.CoSoDaoTaos.CountAsync(),
            TongPhongHoc = await db.PhongHocs.CountAsync(p => p.DeletedAt == null),
            TongHocPhi = await db.HocPhis.CountAsync(),
            KhoaHocChuaCoHocPhi = await db.KhoaHocs.CountAsync(k => !db.HocPhis.Any(h => h.KhoaHocId == k.KhoaHocId)),
            CoSoChuaCoPhong = await db.CoSoDaoTaos.CountAsync(c => !db.PhongHocs.Any(p => p.CoSoId == c.CoSoId && p.DeletedAt == null))
        };
    }

    public async Task<List<CaHoc>> LayDanhSachCaHocAsync()
        => await db.CaHocs
            .AsNoTracking()
            .OrderBy(c => c.GioBatDau)
            .ThenBy(c => c.TenCa)
            .ToListAsync();

    public Task<CaHoc?> LayCaHocTheoIdAsync(int id)
        => db.CaHocs.AsNoTracking().FirstOrDefaultAsync(c => c.CaHocId == id);

    public async Task<ServiceResult> LuuCaHocAsync(CaHoc caHoc, string? nguoiThucHien = null)
    {
        if (string.IsNullOrWhiteSpace(caHoc.TenCa))
            return ThatBai("Tên ca học không được để trống.");
        if (!caHoc.GioBatDau.HasValue || !caHoc.GioKetThuc.HasValue)
            return ThatBai("Vui lòng nhập đầy đủ giờ bắt đầu và giờ kết thúc.");
        if (caHoc.GioBatDau >= caHoc.GioKetThuc)
            return ThatBai("Giờ kết thúc phải sau giờ bắt đầu.");

        var trungTen = await db.CaHocs.AnyAsync(c =>
            c.CaHocId != caHoc.CaHocId
            && c.TenCa != null
            && c.TenCa.ToLower() == caHoc.TenCa.Trim().ToLower());
        if (trungTen)
            return ThatBai("Tên ca học đã tồn tại.");

        if (caHoc.CaHocId == 0)
        {
            caHoc.TenCa = caHoc.TenCa.Trim();
            caHoc.TrangThai = ChuanHoaTrangThaiByte(caHoc.TrangThai);
            caHoc.CreatedAt = DateTime.Now;
            caHoc.UpdatedAt = DateTime.Now;
            db.CaHocs.Add(caHoc);
            await db.SaveChangesAsync();
            await GhiNhatKyAsync("Ca học", "Tạo ca học", $"{caHoc.TenCa} ({caHoc.GioBatDau:HH\\:mm}-{caHoc.GioKetThuc:HH\\:mm})", nguoiThucHien);
            return ThanhCong("Đã thêm ca học.");
        }

        var existing = await db.CaHocs.FirstOrDefaultAsync(c => c.CaHocId == caHoc.CaHocId);
        if (existing == null) return ThatBai("Không tìm thấy ca học.");

        existing.TenCa = caHoc.TenCa.Trim();
        existing.GioBatDau = caHoc.GioBatDau;
        existing.GioKetThuc = caHoc.GioKetThuc;
        existing.MoTa = caHoc.MoTa?.Trim();
        existing.TrangThai = ChuanHoaTrangThaiByte(caHoc.TrangThai);
        existing.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Ca học", "Cập nhật ca học", existing.TenCa, nguoiThucHien);
        return ThanhCong("Đã cập nhật ca học.");
    }

    public async Task<ServiceResult> XoaCaHocAsync(int id, string? nguoiThucHien = null)
    {
        var item = await db.CaHocs.FirstOrDefaultAsync(c => c.CaHocId == id);
        if (item == null) return ThatBai("Không tìm thấy ca học.");

        var dangDung = await db.LopHocs.AnyAsync(l => l.CaHocId == id)
                      || await db.BuoiHocs.AnyAsync(b => b.CaHocId == id);
        if (dangDung)
            return ThatBai("Không thể xóa ca học đang được lớp học hoặc buổi học sử dụng.");

        db.CaHocs.Remove(item);
        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Ca học", "Xóa ca học", item.TenCa, nguoiThucHien);
        return ThanhCong("Đã xóa ca học.");
    }

    public async Task<List<HocPhi>> LayDanhSachHocPhiAsync()
        => await db.HocPhis
            .AsNoTracking()
            .Include(h => h.KhoaHoc)
            .OrderBy(h => h.KhoaHoc!.TenKhoaHoc)
            .ThenBy(h => h.SoBuoi)
            .ToListAsync();

    public Task<HocPhi?> LayHocPhiTheoIdAsync(long id)
        => db.HocPhis.AsNoTracking().FirstOrDefaultAsync(h => h.HocPhiId == id);

    public async Task<ServiceResult> LuuHocPhiAsync(HocPhi hocPhi, string? nguoiThucHien = null)
    {
        if (!hocPhi.KhoaHocId.HasValue || hocPhi.KhoaHocId <= 0)
            return ThatBai("Vui lòng chọn khóa học.");
        if (!hocPhi.SoBuoi.HasValue || hocPhi.SoBuoi <= 0)
            return ThatBai("Số buổi phải lớn hơn 0.");
        if (!hocPhi.DonGia.HasValue || hocPhi.DonGia <= 0)
            return ThatBai("Đơn giá phải lớn hơn 0.");

        var trungGoi = await db.HocPhis.AnyAsync(h =>
            h.HocPhiId != hocPhi.HocPhiId
            && h.KhoaHocId == hocPhi.KhoaHocId
            && h.SoBuoi == hocPhi.SoBuoi);
        if (trungGoi)
            return ThatBai("Khóa học này đã có gói học phí cùng số buổi.");

        if (hocPhi.HocPhiId == 0)
        {
            hocPhi.TrangThai = ChuanHoaTrangThaiByte(hocPhi.TrangThai);
            hocPhi.CreatedAt = DateTime.Now;
            hocPhi.UpdatedAt = DateTime.Now;
            db.HocPhis.Add(hocPhi);
            await db.SaveChangesAsync();
            await GhiNhatKyAsync("Học phí", "Tạo gói học phí", $"Khóa học ID {hocPhi.KhoaHocId} - {hocPhi.SoBuoi} buổi", nguoiThucHien);
            return ThanhCong("Đã thêm gói học phí.");
        }

        var existing = await db.HocPhis.FirstOrDefaultAsync(h => h.HocPhiId == hocPhi.HocPhiId);
        if (existing == null) return ThatBai("Không tìm thấy gói học phí.");

        existing.KhoaHocId = hocPhi.KhoaHocId;
        existing.SoBuoi = hocPhi.SoBuoi;
        existing.DonGia = hocPhi.DonGia;
        existing.TrangThai = ChuanHoaTrangThaiByte(hocPhi.TrangThai);
        existing.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Học phí", "Cập nhật gói học phí", $"ID {existing.HocPhiId}", nguoiThucHien);
        return ThanhCong("Đã cập nhật gói học phí.");
    }

    public async Task<ServiceResult> XoaHocPhiAsync(long id, string? nguoiThucHien = null)
    {
        var item = await db.HocPhis.FirstOrDefaultAsync(h => h.HocPhiId == id);
        if (item == null) return ThatBai("Không tìm thấy gói học phí.");

        var dangDung = await db.LopHocs.AnyAsync(l => l.HocPhiId == id);
        if (dangDung)
            return ThatBai("Không thể xóa gói học phí đang được lớp học sử dụng.");

        db.HocPhis.Remove(item);
        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Học phí", "Xóa gói học phí", $"ID {item.HocPhiId}", nguoiThucHien);
        return ThanhCong("Đã xóa gói học phí.");
    }

    public async Task<List<CoSoDaoTao>> LayDanhSachCoSoAsync()
        => await db.CoSoDaoTaos
            .AsNoTracking()
            .Include(c => c.TinhThanh)
            .OrderBy(c => c.TenCoSo)
            .ToListAsync();

    public Task<CoSoDaoTao?> LayCoSoTheoIdAsync(int id)
        => db.CoSoDaoTaos.AsNoTracking().FirstOrDefaultAsync(c => c.CoSoId == id);

    public async Task<ServiceResult> LuuCoSoAsync(CoSoDaoTao coSo, string? nguoiThucHien = null)
    {
        if (string.IsNullOrWhiteSpace(coSo.TenCoSo))
            return ThatBai("Tên cơ sở không được để trống.");
        if (!coSo.TinhThanhId.HasValue || coSo.TinhThanhId <= 0)
            return ThatBai("Vui lòng chọn tỉnh/thành.");
        if (string.IsNullOrWhiteSpace(coSo.TenPhuongXa))
            return ThatBai("Vui lòng chọn hoặc nhập phường/xã.");

        var tenCoSo = coSo.TenCoSo.Trim();
        var trungTen = await db.CoSoDaoTaos.AnyAsync(c =>
            c.CoSoId != coSo.CoSoId
            && c.TenCoSo.ToLower() == tenCoSo.ToLower());
        if (trungTen)
            return ThatBai("Tên cơ sở đã tồn tại.");

        if (coSo.CoSoId == 0)
        {
            coSo.TenCoSo = tenCoSo;
            coSo.TenPhuongXa = coSo.TenPhuongXa?.Trim();
            coSo.MaCoSo = await TaoMaCoSoAsync();
            coSo.Slug = await TaoSlugAsync(tenCoSo);
            coSo.TrangThai = ChuanHoaTrangThaiByte(coSo.TrangThai);
            coSo.CreatedAt = DateTime.Now;
            coSo.UpdatedAt = DateTime.Now;
            db.CoSoDaoTaos.Add(coSo);
            await db.SaveChangesAsync();
            await GhiNhatKyAsync("Cơ sở", "Tạo cơ sở", coSo.TenCoSo, nguoiThucHien);
            return ThanhCong("Đã thêm cơ sở đào tạo.");
        }

        var existing = await db.CoSoDaoTaos.FirstOrDefaultAsync(c => c.CoSoId == coSo.CoSoId);
        if (existing == null) return ThatBai("Không tìm thấy cơ sở đào tạo.");

        existing.TenCoSo = tenCoSo;
        existing.Slug = await TaoSlugAsync(tenCoSo, existing.CoSoId);
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

    public async Task<ServiceResult> XoaCoSoAsync(int id, string? nguoiThucHien = null)
    {
        var item = await db.CoSoDaoTaos.FirstOrDefaultAsync(c => c.CoSoId == id);
        if (item == null) return ThatBai("Không tìm thấy cơ sở đào tạo.");

        var dangDung = await db.PhongHocs.AnyAsync(p => p.CoSoId == id && p.DeletedAt == null)
                      || await db.LopHocs.AnyAsync(l => l.CoSoId == id)
                      || await db.NhanSus.AnyAsync(n => n.CoSoId == id)
                      || await db.HoaDons.AnyAsync(h => h.CoSoId == id);
        if (dangDung)
            return ThatBai("Không thể xóa cơ sở đang được phòng học, lớp học, nhân sự hoặc hóa đơn sử dụng.");

        db.CoSoDaoTaos.Remove(item);
        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Cơ sở", "Xóa cơ sở", item.TenCoSo, nguoiThucHien);
        return ThanhCong("Đã xóa cơ sở đào tạo.");
    }

    public async Task<List<PhongHoc>> LayDanhSachPhongHocAsync()
        => await db.PhongHocs
            .AsNoTracking()
            .Where(p => p.DeletedAt == null)
            .Include(p => p.CoSo)
            .OrderBy(p => p.CoSo!.TenCoSo)
            .ThenBy(p => p.TenPhong)
            .ToListAsync();

    public Task<PhongHoc?> LayPhongHocTheoIdAsync(int id)
        => db.PhongHocs.AsNoTracking().FirstOrDefaultAsync(p => p.PhongHocId == id && p.DeletedAt == null);

    public async Task<ServiceResult> LuuPhongHocAsync(PhongHoc phongHoc, string? nguoiThucHien = null)
    {
        if (!phongHoc.CoSoId.HasValue || phongHoc.CoSoId <= 0)
            return ThatBai("Vui lòng chọn cơ sở đào tạo.");
        if (string.IsNullOrWhiteSpace(phongHoc.TenPhong))
            return ThatBai("Tên phòng không được để trống.");
        if (!phongHoc.SucChua.HasValue || phongHoc.SucChua <= 1)
            return ThatBai("Sức chứa phải lớn hơn 1.");

        var tenPhong = phongHoc.TenPhong.Trim();
        var trungTen = await db.PhongHocs.AnyAsync(p =>
            p.PhongHocId != phongHoc.PhongHocId
            && p.DeletedAt == null
            && p.CoSoId == phongHoc.CoSoId
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
            await GhiNhatKyAsync("Phòng học", "Tạo phòng học", phongHoc.TenPhong, nguoiThucHien);
            return ThanhCong("Đã thêm phòng học.");
        }

        var existing = await db.PhongHocs.FirstOrDefaultAsync(p => p.PhongHocId == phongHoc.PhongHocId && p.DeletedAt == null);
        if (existing == null) return ThatBai("Không tìm thấy phòng học.");

        existing.CoSoId = phongHoc.CoSoId;
        existing.TenPhong = tenPhong;
        existing.SucChua = phongHoc.SucChua;
        existing.TrangThietBi = phongHoc.TrangThietBi?.Trim();
        existing.MoTa = phongHoc.MoTa?.Trim();
        existing.GhiChuBaoTri = phongHoc.GhiChuBaoTri?.Trim();
        existing.NgayBaoTri = phongHoc.NgayBaoTri;
        existing.TrangThai = ChuanHoaTrangThaiInt(phongHoc.TrangThai);
        existing.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Phòng học", "Cập nhật phòng học", existing.TenPhong, nguoiThucHien);
        return ThanhCong("Đã cập nhật phòng học.");
    }

    public async Task<ServiceResult> XoaPhongHocAsync(int id, string? nguoiThucHien = null)
    {
        var item = await db.PhongHocs.FirstOrDefaultAsync(p => p.PhongHocId == id && p.DeletedAt == null);
        if (item == null) return ThatBai("Không tìm thấy phòng học.");

        var dangDung = await db.LopHocs.AnyAsync(l => l.PhongHocId == id);
        if (dangDung)
            return ThatBai("Không thể xóa phòng học đang được lớp học sử dụng.");

        item.DeletedAt = DateTime.Now;
        item.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        await GhiNhatKyAsync("Phòng học", "Xóa phòng học", item.TenPhong, nguoiThucHien);
        return ThanhCong("Đã xóa phòng học.");
    }

    public async Task<List<KhoaHoc>> LayKhoaHocHoatDongAsync()
        => await db.KhoaHocs
            .AsNoTracking()
            .Where(k => k.DeletedAt == null && k.TrangThai != 0)
            .OrderBy(k => k.TenKhoaHoc)
            .ToListAsync();

    public async Task<List<TinhThanh>> LayTinhThanhAsync()
        => await db.TinhThanhs
            .AsNoTracking()
            .OrderBy(t => t.TenTinhThanh)
            .ToListAsync();

    public async Task<List<CoSoDaoTao>> LayCoSoHoatDongAsync()
        => await db.CoSoDaoTaos
            .AsNoTracking()
            .Where(c => c.TrangThai != 0)
            .OrderBy(c => c.TenCoSo)
            .ToListAsync();

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
    private static int ChuanHoaTrangThaiInt(int trangThai) => trangThai == 0 ? 0 : 1;
    private static ServiceResult ThanhCong(string message) => new() { ThanhCong = true, ThongBao = message };
    private static ServiceResult ThatBai(string message) => new() { ThanhCong = false, ThongBao = message };
}
