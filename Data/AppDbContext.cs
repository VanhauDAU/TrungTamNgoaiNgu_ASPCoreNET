// =============================================================================
// APP DB CONTEXT — KẾT NỐI DATABASE VỚI EF CORE
// =============================================================================
// Đây là "cầu nối" giữa code C# và SQL Server.
// Mỗi DbSet<T> tương ứng với một bảng trong database.
// Entity Framework sẽ tự động tạo SQL queries từ code C#.
// =============================================================================

using Microsoft.EntityFrameworkCore;
using TrungTamNgoaiNgu.Models;

namespace TrungTamNgoaiNgu.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // =========================================================================
    // NHÓM 1: TÀI KHOẢN & PHÂN QUYỀN
    // =========================================================================
    public DbSet<TaiKhoan> TaiKhoans { get; set; }
    public DbSet<HoSoNguoiDung> HoSoNguoiDungs { get; set; }
    public DbSet<NhanSu> NhanSus { get; set; }
    public DbSet<NhomQuyen> NhomQuyens { get; set; }
    public DbSet<PhanQuyen> PhanQuyens { get; set; }

    // =========================================================================
    // NHÓM 2: KHÓA HỌC & LỚP HỌC
    // =========================================================================
    public DbSet<DanhMucKhoaHoc> DanhMucKhoaHocs { get; set; }
    public DbSet<KhoaHoc> KhoaHocs { get; set; }
    public DbSet<HocPhi> HocPhis { get; set; }
    public DbSet<CaHoc> CaHocs { get; set; }
    public DbSet<PhongHoc> PhongHocs { get; set; }
    public DbSet<LopHoc> LopHocs { get; set; }
    public DbSet<DangKyLopHoc> DangKyLopHocs { get; set; }
    public DbSet<BuoiHoc> BuoiHocs { get; set; }
    public DbSet<DiemDanh> DiemDanhs { get; set; }

    // =========================================================================
    // NHÓM 3: TÀI CHÍNH
    // =========================================================================
    public DbSet<HoaDon> HoaDons { get; set; }
    public DbSet<PhieuThu> PhieuThus { get; set; }
    public DbSet<Luong> Luongs { get; set; }
    public DbSet<LuongChiTiet> LuongChiTiets { get; set; }

    // =========================================================================
    // NHÓM 4: CƠ SỞ ĐÀO TẠO
    // =========================================================================
    public DbSet<CoSoDaoTao> CoSoDaoTaos { get; set; }
    public DbSet<TinhThanh> TinhThanhs { get; set; }

    // =========================================================================
    // NHÓM 5: NỘI DUNG & TƯƠNG TÁC
    // =========================================================================
    public DbSet<DanhMucBaiViet> DanhMucBaiViets { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<BaiViet> BaiViets { get; set; }
    public DbSet<LienHe> LienHes { get; set; }
    public DbSet<ThongBao> ThongBaos { get; set; }
    public DbSet<ThongBaoNguoiDung> ThongBaoNguoiDungs { get; set; }
    public DbSet<Setting> Settings { get; set; }

    // =========================================================================
    // NHÓM 6: NHẬT KÝ HỆ THỐNG
    // =========================================================================
    public DbSet<NhatKyHeThong> NhatKyHeThongs { get; set; }

    // =========================================================================
    // CẤU HÌNH MỐI QUAN HỆ GIỮA CÁC BẢNG (Fluent API)
    // Dùng khi Data Annotation chưa đủ để mô tả quan hệ phức tạp
    // =========================================================================
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- TaiKhoan: Khóa ngoài tự tham chiếu / đặc biệt ---
        modelBuilder.Entity<TaiKhoan>()
            .HasOne(tk => tk.HoSo)
            .WithOne(hs => hs.TaiKhoan)
            .HasForeignKey<HoSoNguoiDung>(hs => hs.TaiKhoanId);

        modelBuilder.Entity<TaiKhoan>()
            .HasOne(tk => tk.NhanSu)
            .WithOne(ns => ns.TaiKhoan)
            .HasForeignKey<NhanSu>(ns => ns.TaiKhoanId);

        // --- BaiViet: Many-to-Many với DanhMucBaiViet ---
        modelBuilder.Entity<BaiViet>()
            .HasMany(bv => bv.DanhMucs)
            .WithMany(dm => dm.BaiViets)
            .UsingEntity(j => j.ToTable("baiviet_danhmuc"));

        // --- BaiViet: Many-to-Many với Tag ---
        modelBuilder.Entity<BaiViet>()
            .HasMany(bv => bv.Tags)
            .WithMany(t => t.BaiViets)
            .UsingEntity(j => j.ToTable("baiviet_tag"));

        // --- TaiKhoan: Đặt tên cột taiKhoan (trùng với tên class) ---
        modelBuilder.Entity<TaiKhoan>()
            .Property(tk => tk.TenTaiKhoan)
            .HasColumnName("taiKhoan");

        // --- Cấu hình kiểu dữ liệu decimal cho đúng SQL Server ---
        // Khai báo kiểu decimal cho SQL Server (tránh mất dữ liệu khi lưu)
        modelBuilder.Entity<HoaDon>(e => {
            e.Property(h => h.TongTien).HasColumnType("decimal(15,2)");
            e.Property(h => h.TongTienSauThue).HasColumnType("decimal(15,2)");
            e.Property(h => h.DaTra).HasColumnType("decimal(15,2)");
            e.Property(h => h.GiamGia).HasColumnType("decimal(15,2)");
            e.Property(h => h.Thue).HasColumnType("decimal(5,2)");
        });
        modelBuilder.Entity<HocPhi>().Property(h => h.DonGia).HasColumnType("decimal(15,2)");
        modelBuilder.Entity<LopHoc>().Property(l => l.DonGiaDay).HasColumnType("decimal(15,2)");
        modelBuilder.Entity<Luong>(e => {
            e.Property(l => l.TongLuongDay).HasColumnType("decimal(15,2)");
            e.Property(l => l.TongTienThucLanh).HasColumnType("decimal(15,2)");
            e.Property(l => l.Thuong).HasColumnType("decimal(15,2)");
            e.Property(l => l.Phat).HasColumnType("decimal(15,2)");
            e.Property(l => l.PhuCap).HasColumnType("decimal(15,2)");
        });
        modelBuilder.Entity<LuongChiTiet>(e => {
            e.Property(l => l.DonGiaMotBuoi).HasColumnType("decimal(15,2)");
            e.Property(l => l.TongTien).HasColumnType("decimal(15,2)");
        });
        modelBuilder.Entity<PhieuThu>().Property(p => p.SoTien).HasColumnType("decimal(15,2)");
        modelBuilder.Entity<NhanSu>().Property(n => n.LuongCoBan).HasColumnType("decimal(15,2)");
        modelBuilder.Entity<CoSoDaoTao>(e => {
            e.Property(c => c.ViDo).HasColumnType("decimal(10,7)");
            e.Property(c => c.KinhDo).HasColumnType("decimal(10,7)");
        });
    }
}
