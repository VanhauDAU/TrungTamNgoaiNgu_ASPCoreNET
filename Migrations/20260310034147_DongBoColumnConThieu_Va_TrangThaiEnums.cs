using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <inheritdoc />
    public partial class DongBoColumnConThieu_Va_TrangThaiEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // LƯU Ý QUAN TRỌNG:
            // Do schema DB đã được tạo từ file SQL có sẵn, nên phần lớn các bảng (cahoc, khoahoc,...)
            // đã tồn tại. Thao tác 'migration add' tự sinh lại toàn bộ CreateTable.
            // Để tránh lỗi "There is already an object named 'cahoc' in the database",
            // tôi đã ghi đè Up() chỉ để thực thi thêm các cột CÒN THIẾU.

            // 1. khoahoc: thêm maKhoaHoc
            migrationBuilder.AddColumn<string>(
                name: "maKhoaHoc",
                table: "khoahoc",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            // 2. danhmuckhoahoc: thêm maDanhMuc
            migrationBuilder.AddColumn<string>(
                name: "maDanhMuc",
                table: "danhmuckhoahoc",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            // 3. danhmuckhoahoc: thêm sort_order
            migrationBuilder.AddColumn<long>(
                name: "sort_order",
                table: "danhmuckhoahoc",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            // 4. cahoc: thêm moTa
            migrationBuilder.AddColumn<string>(
                name: "moTa",
                table: "cahoc",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            // 5. lophoc: thêm maLopHoc
            migrationBuilder.AddColumn<string>(
                name: "maLopHoc",
                table: "lophoc",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            // 6. lophoc: thêm deleted_at
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "lophoc",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "maKhoaHoc", table: "khoahoc");
            migrationBuilder.DropColumn(name: "maDanhMuc", table: "danhmuckhoahoc");
            migrationBuilder.DropColumn(name: "sort_order", table: "danhmuckhoahoc");
            migrationBuilder.DropColumn(name: "moTa", table: "cahoc");
            migrationBuilder.DropColumn(name: "maLopHoc", table: "lophoc");
            migrationBuilder.DropColumn(name: "deleted_at", table: "lophoc");
        }
    }
}
