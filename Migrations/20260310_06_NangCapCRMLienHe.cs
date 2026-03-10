using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <summary>
    /// Migration nâng cấp hệ thống CRM Liên Hệ:
    /// 1. Thêm các trường CRM cho bảng lienhe (loại, ghi chú nội bộ, người phụ trách, soft delete)
    /// 2. Tạo bảng lienhe_lichsu (nhật ký hành động CRM)
    /// 3. Tạo bảng lienhe_phanhoi (phản hồi nội bộ / email gửi khách)
    /// </summary>
    public partial class NangCapCRMLienHe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =============================================
            // Nâng cấp bảng lienhe - thêm các trường CRM
            // =============================================

            // Loại liên hệ: tu_van, ho_tro, khieu_nai, khac
            migrationBuilder.AddColumn<string>(
                name: "loaiLienHe",
                table: "lienhe",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "tu_van",
                comment: "tu_van | ho_tro | khieu_nai | khac");

            // Ghi chú nội bộ (không hiển thị với khách hàng)
            migrationBuilder.AddColumn<string>(
                name: "ghiChuNoiBo",
                table: "lienhe",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Ghi chú nội bộ, không hiển thị cho khách hàng");

            // Người phụ trách xử lý liên hệ này
            migrationBuilder.AddColumn<long>(
                name: "nguoiPhuTrachId",
                table: "lienhe",
                type: "bigint",
                nullable: true,
                comment: "FK -> taikhoan.taiKhoanId - Nhân viên phụ trách");

            // Thời điểm xử lý xong
            migrationBuilder.AddColumn<DateTime>(
                name: "thoiGianXuLy",
                table: "lienhe",
                type: "datetime2",
                nullable: true,
                comment: "Thời điểm xử lý xong liên hệ");

            // Soft delete
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "lienhe",
                type: "datetime2",
                nullable: true,
                comment: "Soft delete (chuyển vào thùng rác)");

            // =============================================
            // Bảng lienhe_lichsu: Nhật ký hành động CRM
            // Ghi lại mỗi thao tác: cập nhật trạng thái, ghi chú, gán phụ trách...
            // =============================================
            migrationBuilder.CreateTable(
                name: "lienhe_lichsu",
                columns: table => new
                {
                    lichSuId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lienHeId = table.Column<long>(type: "bigint", nullable: false,
                        comment: "FK -> lienhe.lienHeId"),
                    hanhDong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false,
                        comment: "cap_nhat_trang_thai | ghi_chu | gan_phu_trach | phan_hoi | gui_email | khoi_phuc..."),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: true,
                        comment: "Mô tả chi tiết hành động"),
                    giaTriCu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true,
                        comment: "Giá trị trước khi thay đổi"),
                    giaTriMoi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true,
                        comment: "Giá trị sau khi thay đổi"),
                    nguoiThucHienId = table.Column<long>(type: "bigint", nullable: true,
                        comment: "FK -> taikhoan.taiKhoanId - Người thực hiện"),
                    tenNguoiThucHien = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true,
                        comment: "Cache tên người thực hiện (tránh query thêm khi người này bị xóa)"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false,
                        defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lienhe_lichsu", x => x.lichSuId);
                });

            migrationBuilder.CreateIndex("IX_lienhe_lichsu_lienHeId", "lienhe_lichsu", "lienHeId");

            // =============================================
            // Bảng lienhe_phanhoi: Nội dung phản hồi
            // Ghi lại tin nhắn nội bộ hoặc email gửi cho khách
            // =============================================
            migrationBuilder.CreateTable(
                name: "lienhe_phanhoi",
                columns: table => new
                {
                    phanHoiId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lienHeId = table.Column<long>(type: "bigint", nullable: false,
                        comment: "FK -> lienhe.lienHeId"),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: false,
                        comment: "Nội dung phản hồi"),
                    loai = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false,
                        defaultValue: "noi_bo",
                        comment: "noi_bo = ghi chú nội bộ | email = đã gửi email cho khách"),
                    nguoiGuiId = table.Column<long>(type: "bigint", nullable: true,
                        comment: "FK -> taikhoan.taiKhoanId"),
                    tenNguoiGui = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true,
                        comment: "Cache tên người gửi"),
                    daGuiEmail = table.Column<bool>(type: "bit", nullable: false, defaultValue: false,
                        comment: "true = đã gửi email thực sự ra ngoài"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lienhe_phanhoi", x => x.phanHoiId);
                    table.ForeignKey(
                        name: "FK_lienhe_phanhoi_lienhe_lienHeId",
                        column: x => x.lienHeId,
                        principalTable: "lienhe",
                        principalColumn: "lienHeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex("IX_lienhe_phanhoi_lienHeId", "lienhe_phanhoi", "lienHeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("lienhe_phanhoi");
            migrationBuilder.DropTable("lienhe_lichsu");

            migrationBuilder.DropColumn("loaiLienHe", "lienhe");
            migrationBuilder.DropColumn("ghiChuNoiBo", "lienhe");
            migrationBuilder.DropColumn("nguoiPhuTrachId", "lienhe");
            migrationBuilder.DropColumn("thoiGianXuLy", "lienhe");
            migrationBuilder.DropColumn("deleted_at", "lienhe");
        }
    }
}
