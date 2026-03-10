using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <summary>
    /// Migration thêm 2 tính năng bảo mật:
    /// 1. Bảng nhatky_dangnhap: lưu lịch sử đăng nhập (thành công/thất bại, IP, user agent)
    /// 2. Cột phaiDoiMatKhau trong bảng taikhoan: buộc đổi mật khẩu lần đầu đăng nhập
    /// </summary>
    public partial class BaoMatDangNhap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =============================================
            // Bảng nhatky_dangnhap: Nhật ký đăng nhập
            // Ghi lại mỗi lần đăng nhập (cả thành công và thất bại)
            // Dùng để phát hiện brute-force, đăng nhập bất thường
            // =============================================
            migrationBuilder.CreateTable(
                name: "nhatky_dangnhap",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    taiKhoan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false,
                        comment: "Tên đăng nhập hoặc email đã nhập (không cần tồn tại trong hệ thống)"),
                    ip = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true,
                        comment: "Địa chỉ IP (IPv4 hoặc IPv6, tối đa 45 ký tự)"),
                    thanhCong = table.Column<bool>(type: "bit", nullable: false, defaultValue: false,
                        comment: "true = đăng nhập thành công, false = thất bại"),
                    userAgent = table.Column<string>(type: "nvarchar(max)", nullable: true,
                        comment: "Chuỗi User-Agent (trình duyệt, hệ điều hành)"),
                    thoiGian = table.Column<DateTime>(type: "datetime2", nullable: false,
                        defaultValueSql: "GETDATE()",
                        comment: "Thời điểm đăng nhập")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nhatky_dangnhap", x => x.id);
                });

            // Index tra cứu nhanh theo tài khoản + kết quả + thời gian (phát hiện brute force)
            migrationBuilder.CreateIndex(
                name: "IX_nhatky_dangnhap_taikhoan_thanhcong_thoigian",
                table: "nhatky_dangnhap",
                columns: new[] { "taiKhoan", "thanhCong", "thoiGian" });

            // Index tra cứu theo IP (phát hiện tấn công từ cùng IP)
            migrationBuilder.CreateIndex(
                name: "IX_nhatky_dangnhap_ip_thanhcong_thoigian",
                table: "nhatky_dangnhap",
                columns: new[] { "ip", "thanhCong", "thoiGian" });

            // =============================================
            // Thêm cột phaiDoiMatKhau vào bảng taikhoan
            // Dùng khi admin tạo tài khoản mới hoặc reset mật khẩu
            // Giá trị 1 = buộc đổi mật khẩu ngay khi đăng nhập
            // =============================================
            migrationBuilder.AddColumn<byte>(
                name: "phaiDoiMatKhau",
                table: "taikhoan",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                comment: "1 = phải đổi mật khẩu khi đăng nhập lần đầu, 0 = không");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("nhatky_dangnhap");
            migrationBuilder.DropColumn("phaiDoiMatKhau", "taikhoan");
        }
    }
}
