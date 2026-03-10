using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <summary>
    /// Migration đồng bộ các cột bị thiếu so với SQL gốc:
    /// 1. khoahoc.maKhoaHoc    — varchar(20), mã khóa học (VD: KH001)
    /// 2. danhmuckhoahoc.maDanhMuc — varchar(20), mã danh mục (VD: TA, TN)
    /// 3. danhmuckhoahoc.sort_order — int unsigned NOT NULL DEFAULT 0, thứ tự hiển thị
    /// 4. cahoc.moTa           — varchar(255), mô tả ca học
    /// 5. lophoc.maLopHoc      — varchar(20), mã lớp học (đã có trong Model, chưa có migration)
    /// 6. lophoc.deleted_at    — timestamp NULL, soft delete (đã có trong Model)
    /// </summary>
    public partial class DongBoColumnConThieu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ─── 1. khoahoc: thêm maKhoaHoc ───────────────────────────────────────────
            migrationBuilder.AddColumn<string>(
                name: "maKhoaHoc",
                table: "khoahoc",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            // ─── 2. danhmuckhoahoc: thêm maDanhMuc ────────────────────────────────────
            migrationBuilder.AddColumn<string>(
                name: "maDanhMuc",
                table: "danhmuckhoahoc",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            // ─── 3. danhmuckhoahoc: thêm sort_order ───────────────────────────────────
            migrationBuilder.AddColumn<uint>(
                name: "sort_order",
                table: "danhmuckhoahoc",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u);

            // ─── 4. cahoc: thêm moTa ──────────────────────────────────────────────────
            migrationBuilder.AddColumn<string>(
                name: "moTa",
                table: "cahoc",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            // ─── 5. lophoc: thêm maLopHoc (đã có trong Model từ migration trước nhưng
            //       chưa có migration thực thi thêm cột này vào DB) ────────────────────
            migrationBuilder.AddColumn<string>(
                name: "maLopHoc",
                table: "lophoc",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            // ─── 6. lophoc: thêm deleted_at ───────────────────────────────────────────
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "lophoc",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "maKhoaHoc",  table: "khoahoc");
            migrationBuilder.DropColumn(name: "maDanhMuc",  table: "danhmuckhoahoc");
            migrationBuilder.DropColumn(name: "sort_order", table: "danhmuckhoahoc");
            migrationBuilder.DropColumn(name: "moTa",       table: "cahoc");
            migrationBuilder.DropColumn(name: "maLopHoc",   table: "lophoc");
            migrationBuilder.DropColumn(name: "deleted_at", table: "lophoc");
        }
    }
}
