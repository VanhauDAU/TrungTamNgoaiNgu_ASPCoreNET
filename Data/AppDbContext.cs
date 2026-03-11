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
    public DbSet<BaiThi> BaiThis { get; set; }
    public DbSet<DiemBaiThi> DiemBaiThis { get; set; }

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
    public DbSet<LienHeLichSu> LienHeLichSus { get; set; }
    public DbSet<LienHePhanHoi> LienHePhanHois { get; set; }
    public DbSet<DanhGiaGiaoVien> DanhGiaGiaoViens { get; set; }
    public DbSet<PhanHoi> PhanHois { get; set; }
    public DbSet<TaiLieu> TaiLieus { get; set; }
    public DbSet<NoiDungBaiHoc> NoiDungBaiHocs { get; set; }
    public DbSet<ThongBao> ThongBaos { get; set; }
    public DbSet<ThongBaoNguoiDung> ThongBaoNguoiDungs { get; set; }
    public DbSet<ThongBaoTepDinh> ThongBaoTepDinhs { get; set; }
    public DbSet<Setting> Settings { get; set; }

    // =========================================================================
    // NHÓM 6: NHẬT KÝ HỆ THỐNG
    // =========================================================================
    public DbSet<NhatKyHeThong> NhatKyHeThongs { get; set; }

    // =========================================================================
    // NHÓM 7: CHAT
    // =========================================================================
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<ChatRoomMember> ChatRoomMembers { get; set; }
    public DbSet<ChatMessageReaction> ChatMessageReactions { get; set; }
    public DbSet<ChatMessageAttachment> ChatMessageAttachments { get; set; }
    public DbSet<ChatMessageDelete> ChatMessageDeletes { get; set; }
    public DbSet<ChatAuditLog> ChatAuditLogs { get; set; }

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

        // Danh mục khóa học hỗ trợ cây cha-con.
        modelBuilder.Entity<DanhMucKhoaHoc>()
            .HasOne(dm => dm.Parent)
            .WithMany(dm => dm.Children)
            .HasForeignKey(dm => dm.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<DanhMucKhoaHoc>()
            .HasIndex(dm => dm.ParentId)
            .HasDatabaseName("idx_danhmuc_parent");

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
        modelBuilder.Entity<DiemBaiThi>().Property(d => d.DiemSo).HasColumnType("decimal(4,2)");
        modelBuilder.Entity<PhieuThu>().Property(p => p.SoTien).HasColumnType("decimal(15,2)");
        modelBuilder.Entity<NhanSu>().Property(n => n.LuongCoBan).HasColumnType("decimal(15,2)");
        modelBuilder.Entity<CoSoDaoTao>(e => {
            e.Property(c => c.ViDo).HasColumnType("decimal(10,7)");
            e.Property(c => c.KinhDo).HasColumnType("decimal(10,7)");
        });

        // Contact tracking extension tables (theo SQL (10))
        modelBuilder.Entity<LienHe>()
            .Property(x => x.LoaiLienHe)
            .HasDefaultValue("tu_van");
        modelBuilder.Entity<LienHeLichSu>().HasIndex(x => x.LienHeId);
        modelBuilder.Entity<LienHePhanHoi>().HasIndex(x => x.LienHeId);
        modelBuilder.Entity<LienHePhanHoi>()
            .Property(x => x.Loai)
            .HasDefaultValue("noi_bo");
        modelBuilder.Entity<LienHePhanHoi>()
            .Property(x => x.DaGuiEmail)
            .HasDefaultValue(false);
        modelBuilder.Entity<ThongBaoTepDinh>().HasIndex(x => x.ThongBaoId);

        // =========================================================================
        // CHAT: Cấu hình mối quan hệ
        // =========================================================================
        // ChatRoom -> LopHoc (1-1)
        modelBuilder.Entity<ChatRoom>()
            .HasOne(r => r.LopHoc)
            .WithMany()
            .HasForeignKey(r => r.LopHocId)
            .OnDelete(DeleteBehavior.SetNull);

        // ChatRoom -> TaiKhoan (người tạo)
        modelBuilder.Entity<ChatRoom>()
            .HasOne(r => r.TaoBoiTaiKhoan)
            .WithMany()
            .HasForeignKey(r => r.TaoBoiId)
            .OnDelete(DeleteBehavior.SetNull);

        // ChatMessage -> ChatRoom
        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.ChatRoom)
            .WithMany(r => r.Messages)
            .HasForeignKey(m => m.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);

        // ChatMessage -> TaiKhoan (người gửi)
        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.NguoiGui)
            .WithMany()
            .HasForeignKey(m => m.NguoiGuiId)
            .OnDelete(DeleteBehavior.NoAction);

        // ChatMessage -> ChatMessage (reply, self-ref)
        modelBuilder.Entity<ChatMessage>()
            .HasOne(m => m.ReplyToMessage)
            .WithMany(m => m.Replies)
            .HasForeignKey(m => m.ReplyToMessageId)
            .OnDelete(DeleteBehavior.NoAction); // NoAction để tránh multiple cascade paths


        // ChatRoomMember: Unique (chatRoomId, taiKhoanId)
        modelBuilder.Entity<ChatRoomMember>()
            .HasIndex(x => new { x.ChatRoomId, x.TaiKhoanId })
            .IsUnique()
            .HasDatabaseName("UQ_chat_room_members_room_user");

        modelBuilder.Entity<ChatRoomMember>()
            .HasOne(m => m.ChatRoom)
            .WithMany(r => r.Members)
            .HasForeignKey(m => m.ChatRoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatRoomMember>()
            .HasOne(m => m.TaiKhoan)
            .WithMany()
            .HasForeignKey(m => m.TaiKhoanId)
            .OnDelete(DeleteBehavior.Cascade);

        // ChatMessageReaction: Unique (chatMessageId, taiKhoanId, emoji)
        modelBuilder.Entity<ChatMessageReaction>()
            .HasIndex(x => new { x.ChatMessageId, x.TaiKhoanId, x.Emoji })
            .IsUnique()
            .HasDatabaseName("UQ_chat_message_reactions_msg_user_emoji");

        modelBuilder.Entity<ChatMessageReaction>()
            .HasOne(r => r.ChatMessage)
            .WithMany(m => m.Reactions)
            .HasForeignKey(r => r.ChatMessageId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatMessageReaction>()
            .HasOne(r => r.TaiKhoan)
            .WithMany()
            .HasForeignKey(r => r.TaiKhoanId)
            .OnDelete(DeleteBehavior.Cascade);

        // ChatMessageAttachment -> ChatMessage
        modelBuilder.Entity<ChatMessageAttachment>()
            .HasOne(a => a.ChatMessage)
            .WithMany(m => m.Attachments)
            .HasForeignKey(a => a.ChatMessageId)
            .OnDelete(DeleteBehavior.Cascade);

        // ChatMessageDelete: Unique (chatMessageId, taiKhoanId)
        modelBuilder.Entity<ChatMessageDelete>()
            .HasIndex(x => new { x.ChatMessageId, x.TaiKhoanId })
            .IsUnique()
            .HasDatabaseName("UQ_chat_message_deletes_msg_user");

        modelBuilder.Entity<ChatMessageDelete>()
            .HasOne(d => d.ChatMessage)
            .WithMany(m => m.Deletes)
            .HasForeignKey(d => d.ChatMessageId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatMessageDelete>()
            .HasOne(d => d.TaiKhoan)
            .WithMany()
            .HasForeignKey(d => d.TaiKhoanId)
            .OnDelete(DeleteBehavior.Cascade);

        // ChatAuditLog
        modelBuilder.Entity<ChatAuditLog>()
            .HasOne(a => a.ChatRoom)
            .WithMany(r => r.AuditLogs)
            .HasForeignKey(a => a.ChatRoomId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ChatAuditLog>()
            .HasOne(a => a.TaiKhoan)
            .WithMany()
            .HasForeignKey(a => a.TaiKhoanId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
