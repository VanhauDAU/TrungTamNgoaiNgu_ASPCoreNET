using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <summary>
    /// Migration cập nhật bảng phonghoc:
    /// 1. Thêm soft delete (deleted_at)
    /// 2. Thêm thông tin bảo trì (ghiChuBaoTri, ngayBaoTri)
    /// 3. Chuẩn hóa trangThai thành 4 trạng thái rõ ràng:
    ///    0=Đang bảo trì, 1=Hoạt động, 2=Tạm ngưng, 3=Ngưng hẳn
    /// </summary>
    public partial class PhongHocBaoTriVaSoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm cột mô tả phòng chi tiết hơn
            migrationBuilder.AddColumn<string>(
                name: "moTa",
                table: "phonghoc",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "Mô tả thêm về phòng học (vị trí, đặc điểm...)");

            // Thêm cột ghi chú bảo trì
            migrationBuilder.AddColumn<string>(
                name: "ghiChuBaoTri",
                table: "phonghoc",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Lý do / nội dung bảo trì phòng học");

            // Thêm cột ngày bảo trì
            migrationBuilder.AddColumn<DateTime>(
                name: "ngayBaoTri",
                table: "phonghoc",
                type: "datetime2",
                nullable: true,
                comment: "Thời điểm bắt đầu bảo trì");

            // Thêm soft delete
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "phonghoc",
                type: "datetime2",
                nullable: true,
                comment: "Soft delete: thời điểm xóa mềm. NULL = chưa xóa");

            // Cập nhật comment cho cột trangThai (chuẩn hóa 4 trạng thái)
            // 0 = Đang bảo trì, 1 = Hoạt động bình thường, 2 = Tạm ngưng, 3 = Đã ngưng
            migrationBuilder.AlterColumn<int>(
                name: "trangThai",
                table: "phonghoc",
                type: "int",
                nullable: false,
                defaultValue: 1,
                comment: "0=Đang bảo trì, 1=Hoạt động, 2=Tạm ngưng, 3=Ngưng hẳn",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("moTa", "phonghoc");
            migrationBuilder.DropColumn("ghiChuBaoTri", "phonghoc");
            migrationBuilder.DropColumn("ngayBaoTri", "phonghoc");
            migrationBuilder.DropColumn("deleted_at", "phonghoc");
        }
    }
}
