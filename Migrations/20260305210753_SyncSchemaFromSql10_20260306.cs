using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    /// <inheritdoc />
    public partial class SyncSchemaFromSql10_20260306 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_thongbaonguoidung_thongbao_thongBaoId",
                table: "thongbaonguoidung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_thongbaonguoidung",
                table: "thongbaonguoidung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_thongbao",
                table: "thongbao");

            migrationBuilder.AlterColumn<int>(
                name: "thongBaoId",
                table: "thongbaonguoidung",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "thongBaoNguoiDungId",
                table: "thongbaonguoidung",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "doiTuongId",
                table: "thongbao",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "thongBaoId",
                table: "thongbao",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_thongbao",
                table: "thongbao",
                column: "thongBaoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_thongbaonguoidung",
                table: "thongbaonguoidung",
                column: "thongBaoNguoiDungId");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "lienhe",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ghiChuNoiBo",
                table: "lienhe",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "loaiLienHe",
                table: "lienhe",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "tu_van");

            migrationBuilder.AddColumn<long>(
                name: "nguoiPhuTrachId",
                table: "lienhe",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "thoiGianXuLy",
                table: "lienhe",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "danhmuckhoahoc",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tenCa",
                table: "cahoc",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "moTa",
                table: "cahoc",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "lienhe_lichsu",
                columns: table => new
                {
                    lichSuId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lienHeId = table.Column<long>(type: "bigint", nullable: false),
                    hanhDong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    giaTriCu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    giaTriMoi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    nguoiThucHienId = table.Column<long>(type: "bigint", nullable: true),
                    tenNguoiThucHien = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lienhe_lichsu", x => x.lichSuId);
                });

            migrationBuilder.CreateTable(
                name: "lienhe_phanhoi",
                columns: table => new
                {
                    phanHoiId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lienHeId = table.Column<long>(type: "bigint", nullable: false),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "noi_bo"),
                    nguoiGuiId = table.Column<long>(type: "bigint", nullable: true),
                    tenNguoiGui = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    daGuiEmail = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lienhe_phanhoi", x => x.phanHoiId);
                });

            migrationBuilder.CreateTable(
                name: "thongbao_tepdinh",
                columns: table => new
                {
                    tepDinhId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    thongBaoId = table.Column<int>(type: "int", nullable: false),
                    tenFile = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    tenFileLuu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    duongDan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    loaiFile = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    kichThuoc = table.Column<long>(type: "bigint", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_danhmuckhoahoc_parent_id",
                table: "danhmuckhoahoc",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_lienhe_lichsu_lienHeId",
                table: "lienhe_lichsu",
                column: "lienHeId");

            migrationBuilder.CreateIndex(
                name: "IX_lienhe_phanhoi_lienHeId",
                table: "lienhe_phanhoi",
                column: "lienHeId");

            migrationBuilder.CreateIndex(
                name: "IX_thongbao_tepdinh_thongBaoId",
                table: "thongbao_tepdinh",
                column: "thongBaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_danhmuckhoahoc_danhmuckhoahoc_parent_id",
                table: "danhmuckhoahoc",
                column: "parent_id",
                principalTable: "danhmuckhoahoc",
                principalColumn: "danhMucId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_thongbaonguoidung_thongbao_thongBaoId",
                table: "thongbaonguoidung",
                column: "thongBaoId",
                principalTable: "thongbao",
                principalColumn: "thongBaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_danhmuckhoahoc_danhmuckhoahoc_parent_id",
                table: "danhmuckhoahoc");

            migrationBuilder.DropForeignKey(
                name: "FK_thongbaonguoidung_thongbao_thongBaoId",
                table: "thongbaonguoidung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_thongbaonguoidung",
                table: "thongbaonguoidung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_thongbao",
                table: "thongbao");

            migrationBuilder.DropTable(
                name: "lienhe_lichsu");

            migrationBuilder.DropTable(
                name: "lienhe_phanhoi");

            migrationBuilder.DropTable(
                name: "thongbao_tepdinh");

            migrationBuilder.DropIndex(
                name: "IX_danhmuckhoahoc_parent_id",
                table: "danhmuckhoahoc");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "lienhe");

            migrationBuilder.DropColumn(
                name: "ghiChuNoiBo",
                table: "lienhe");

            migrationBuilder.DropColumn(
                name: "loaiLienHe",
                table: "lienhe");

            migrationBuilder.DropColumn(
                name: "nguoiPhuTrachId",
                table: "lienhe");

            migrationBuilder.DropColumn(
                name: "thoiGianXuLy",
                table: "lienhe");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "danhmuckhoahoc");

            migrationBuilder.DropColumn(
                name: "moTa",
                table: "cahoc");

            migrationBuilder.AlterColumn<long>(
                name: "thongBaoId",
                table: "thongbaonguoidung",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "thongBaoNguoiDungId",
                table: "thongbaonguoidung",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "doiTuongId",
                table: "thongbao",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "thongBaoId",
                table: "thongbao",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_thongbao",
                table: "thongbao",
                column: "thongBaoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_thongbaonguoidung",
                table: "thongbaonguoidung",
                column: "thongBaoNguoiDungId");

            migrationBuilder.AlterColumn<string>(
                name: "tenCa",
                table: "cahoc",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_thongbaonguoidung_thongbao_thongBaoId",
                table: "thongbaonguoidung",
                column: "thongBaoId",
                principalTable: "thongbao",
                principalColumn: "thongBaoId");
        }
    }
}
