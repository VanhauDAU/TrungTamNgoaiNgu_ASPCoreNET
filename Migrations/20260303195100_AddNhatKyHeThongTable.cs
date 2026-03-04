using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TrungTamNgoaiNgu.Data;

#nullable disable

namespace TrungTamNgoaiNgu.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260303195100_AddNhatKyHeThongTable")]
    public partial class AddNhatKyHeThongTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nhatkyhethong",
                columns: table => new
                {
                    nhatKyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    module = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    hanhDong = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    noiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nguoiThucHien = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nhatkyhethong", x => x.nhatKyId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nhatkyhethong");
        }
    }
}
