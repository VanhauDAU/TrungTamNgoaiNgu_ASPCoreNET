using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <summary>
    /// Migration nâng cấp hệ thống Thông báo:
    /// - Thêm các trường quản lý gửi (sendTrangThai, scheduled_at, sent_at, failed_at, failure_reason)
    /// - Thêm loaiGui, uuTien, ghim, hinhAnh
    /// - Thêm soft delete (deleted_at)
    /// - Tạo bảng thongbao_lichsu (nhật ký hành động)
    /// - Tạo bảng thongbao_tepdinh (file đính kèm)
    /// </summary>
    public partial class NangCapHeThongThongBao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =============================================
            // Nâng cấp bảng thongbao - thêm nhiều trường quan trọng
            // =============================================

            // Loại gửi: 0=Hệ thống, 1=Học tập, 2=Tài chính, 3=Sự kiện, 4=Khẩn cấp
            migrationBuilder.AddColumn<byte>(
                name: "loaiGui",
                table: "thongbao",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                comment: "0=Hệ thống, 1=Học tập, 2=Tài chính, 3=Sự kiện, 4=Khẩn cấp");

            // Độ ưu tiên: 0=Bình thường, 1=Quan trọng, 2=Khẩn cấp
            migrationBuilder.AddColumn<byte>(
                name: "uuTien",
                table: "thongbao",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                comment: "0=Bình thường, 1=Quan trọng, 2=Khẩn cấp");

            // Ghim thông báo lên đầu
            migrationBuilder.AddColumn<bool>(
                name: "ghim",
                table: "thongbao",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "true = ghim thông báo này lên đầu danh sách");

            // Hình ảnh đính kèm thông báo
            migrationBuilder.AddColumn<string>(
                name: "hinhAnh",
                table: "thongbao",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            // Trạng thái gửi: 0=Nháp, 1=Đã lên lịch, 2=Đã gửi, 3=Gửi lỗi
            migrationBuilder.AddColumn<byte>(
                name: "sendTrangThai",
                table: "thongbao",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)2,
                comment: "0=Nháp, 1=Đã lên lịch, 2=Đã gửi, 3=Gửi lỗi");

            // Thời điểm lên lịch gửi
            migrationBuilder.AddColumn<DateTime>(
                name: "scheduled_at",
                table: "thongbao",
                type: "datetime2",
                nullable: true,
                comment: "Thời điểm lên lịch gửi (null = gửi ngay)");

            // Thời điểm đã gửi thành công
            migrationBuilder.AddColumn<DateTime>(
                name: "sent_at",
                table: "thongbao",
                type: "datetime2",
                nullable: true);

            // Thời điểm gửi thất bại
            migrationBuilder.AddColumn<DateTime>(
                name: "failed_at",
                table: "thongbao",
                type: "datetime2",
                nullable: true);

            // Lý do gửi thất bại
            migrationBuilder.AddColumn<string>(
                name: "failure_reason",
                table: "thongbao",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            // Soft delete
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "thongbao",
                type: "datetime2",
                nullable: true,
                comment: "Soft delete: thông báo vào thùng rác");

            // Index tra cứu
            migrationBuilder.CreateIndex(
                name: "IX_thongbao_sendTrangThai",
                table: "thongbao",
                column: "sendTrangThai");

            migrationBuilder.CreateIndex(
                name: "IX_thongbao_scheduled_at",
                table: "thongbao",
                column: "scheduled_at");

            // =============================================
            // Bảng thongbao_lichsu: Nhật ký thao tác trên thông báo
            // =============================================
            migrationBuilder.CreateTable(
                name: "thongbao_lichsu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    thongBaoId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> thongbao.thongBaoId (null nếu thông báo đã xóa vĩnh viễn)"),
                    taiKhoanId = table.Column<int>(type: "int", nullable: true,
                        comment: "FK -> taikhoan.taiKhoanId - Người thực hiện"),
                    hanhDong = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false,
                        comment: "draft_created | sent | scheduled | deleted | duplicated | updated | test_sent..."),
                    moTa = table.Column<string>(type: "nvarchar(max)", nullable: true,
                        comment: "Mô tả chi tiết hành động"),
                    payload = table.Column<string>(type: "nvarchar(max)", nullable: true,
                        comment: "Dữ liệu bổ sung dạng JSON"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false,
                        defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thongbao_lichsu", x => x.id);
                    table.ForeignKey(
                        name: "FK_thongbao_lichsu_thongbao_thongBaoId",
                        column: x => x.thongBaoId,
                        principalTable: "thongbao",
                        principalColumn: "thongBaoId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_thongbao_lichsu_taikhoan_taiKhoanId",
                        column: x => x.taiKhoanId,
                        principalTable: "taikhoan",
                        principalColumn: "taiKhoanId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex("IX_thongbao_lichsu_thongBaoId", "thongbao_lichsu", "thongBaoId");
            migrationBuilder.CreateIndex("IX_thongbao_lichsu_taiKhoanId", "thongbao_lichsu", "taiKhoanId");

            // =============================================
            // Bảng thongbao_tepdinh: File đính kèm trong thông báo
            // =============================================
            migrationBuilder.CreateTable(
                name: "thongbao_tepdinh",
                columns: table => new
                {
                    tepDinhId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    thongBaoId = table.Column<int>(type: "int", nullable: false,
                        comment: "FK -> thongbao.thongBaoId"),
                    tenFile = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false,
                        comment: "Tên file gốc của người dùng"),
                    tenFileLuu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false,
                        comment: "Tên file lưu trên server (uuid + extension)"),
                    duongDan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false,
                        comment: "Đường dẫn tương đối trong storage/public"),
                    loaiFile = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true,
                        comment: "MIME type (text/plain, application/pdf, image/jpeg...)"),
                    kichThuoc = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L,
                        comment: "Kích thước file tính bằng byte"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thongbao_tepdinh", x => x.tepDinhId);
                    table.ForeignKey(
                        name: "FK_thongbao_tepdinh_thongbao_thongBaoId",
                        column: x => x.thongBaoId,
                        principalTable: "thongbao",
                        principalColumn: "thongBaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex("IX_thongbao_tepdinh_thongBaoId", "thongbao_tepdinh", "thongBaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("thongbao_tepdinh");
            migrationBuilder.DropTable("thongbao_lichsu");

            migrationBuilder.DropIndex("IX_thongbao_sendTrangThai", "thongbao");
            migrationBuilder.DropIndex("IX_thongbao_scheduled_at", "thongbao");

            migrationBuilder.DropColumn("loaiGui", "thongbao");
            migrationBuilder.DropColumn("uuTien", "thongbao");
            migrationBuilder.DropColumn("ghim", "thongbao");
            migrationBuilder.DropColumn("hinhAnh", "thongbao");
            migrationBuilder.DropColumn("sendTrangThai", "thongbao");
            migrationBuilder.DropColumn("scheduled_at", "thongbao");
            migrationBuilder.DropColumn("sent_at", "thongbao");
            migrationBuilder.DropColumn("failed_at", "thongbao");
            migrationBuilder.DropColumn("failure_reason", "thongbao");
            migrationBuilder.DropColumn("deleted_at", "thongbao");
        }
    }
}
